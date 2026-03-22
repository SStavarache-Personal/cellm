using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Cellm.Models.Providers.FoundryLocal;

internal class FoundryLocalModelManager(ILogger<FoundryLocalModelManager> logger) : IDisposable
{
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _modelLocks = new();
    private readonly ConcurrentDictionary<string, bool> _readyModels = new();

    private Microsoft.AI.Foundry.Local.FoundryLocalManager? _manager;
    private Microsoft.AI.Foundry.Local.ICatalog? _catalog;
    private List<string>? _cachedModelAliases;
    private Uri? _baseAddress;
    private bool _disposed;

    internal async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_manager is not null)
        {
            return;
        }

        await _initLock.WaitAsync(ct);

        try
        {
            if (_manager is not null)
            {
                return;
            }

            logger.LogInformation("Initializing Foundry Local manager...");

            var config = new Microsoft.AI.Foundry.Local.Configuration
            {
                AppName = "Cellm",
                LogLevel = Microsoft.AI.Foundry.Local.LogLevel.Warning
            };

            await Microsoft.AI.Foundry.Local.FoundryLocalManager.CreateAsync(config, logger, ct);
            _manager = Microsoft.AI.Foundry.Local.FoundryLocalManager.Instance;

            logger.LogInformation("Starting Foundry Local web service...");
            await _manager.StartWebServiceAsync(ct);

            if (_manager.Urls is { Length: > 0 })
            {
                _baseAddress = new Uri(_manager.Urls[0]);
                logger.LogInformation("Foundry Local web service started at {baseAddress}.", _baseAddress);
            }
            else
            {
                throw new InvalidOperationException("Foundry Local web service started but no URLs were returned.");
            }

            _catalog = await _manager.GetCatalogAsync(ct);

            logger.LogInformation("Foundry Local initialized successfully.");
        }
        finally
        {
            _initLock.Release();
        }
    }

    internal Uri GetBaseAddress()
    {
        return _baseAddress ?? throw new InvalidOperationException("Foundry Local is not initialized. Call InitializeAsync first.");
    }

    internal async Task<List<string>> GetCatalogModelAliasesAsync(CancellationToken ct = default)
    {
        await InitializeAsync(ct);

        if (_cachedModelAliases is not null)
        {
            return _cachedModelAliases;
        }

        var models = await _catalog!.ListModelsAsync(ct);

        _cachedModelAliases = models
            .Select(m => m.Alias)
            .Where(alias => !string.IsNullOrEmpty(alias))
            .Order()
            .ToList();

        logger.LogInformation("Found {count} models in Foundry Local catalog.", _cachedModelAliases.Count);

        return _cachedModelAliases;
    }

    internal List<string> GetCatalogModelAliases()
    {
        if (_cachedModelAliases is not null)
        {
            return _cachedModelAliases;
        }

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            return GetCatalogModelAliasesAsync(cts.Token).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            logger.LogWarning("Could not fetch Foundry Local catalog: {message}", ex.Message);
            return [];
        }
    }

    internal async Task EnsureModelReadyAsync(string modelAlias, CancellationToken ct = default)
    {
        await InitializeAsync(ct);

        if (_readyModels.ContainsKey(modelAlias))
        {
            return;
        }

        var modelLock = _modelLocks.GetOrAdd(modelAlias, _ => new SemaphoreSlim(1, 1));
        await modelLock.WaitAsync(ct);

        try
        {
            if (_readyModels.ContainsKey(modelAlias))
            {
                return;
            }

            var model = await _catalog!.GetModelAsync(modelAlias, ct)
                ?? throw new InvalidOperationException($"Model '{modelAlias}' not found in Foundry Local catalog.");

            var isCached = await model.IsCachedAsync(ct);

            if (!isCached)
            {
                logger.LogInformation("Downloading model '{alias}'...", modelAlias);
                await model.DownloadAsync(
                    progress => logger.LogInformation("Downloading '{alias}': {progress:P0}", modelAlias, progress),
                    ct);
                logger.LogInformation("Model '{alias}' downloaded.", modelAlias);
            }

            var isLoaded = await model.IsLoadedAsync(ct);

            if (!isLoaded)
            {
                logger.LogInformation("Loading model '{alias}'...", modelAlias);
                await model.LoadAsync(ct);
                logger.LogInformation("Model '{alias}' loaded.", modelAlias);
            }

            _readyModels.TryAdd(modelAlias, true);
        }
        finally
        {
            modelLock.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            if (_manager is not null)
            {
                _manager.StopWebServiceAsync().GetAwaiter().GetResult();
                _manager.Dispose();
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning("Error disposing Foundry Local manager: {message}", ex.Message);
        }

        _initLock.Dispose();

        foreach (var semaphore in _modelLocks.Values)
        {
            semaphore.Dispose();
        }
    }
}
