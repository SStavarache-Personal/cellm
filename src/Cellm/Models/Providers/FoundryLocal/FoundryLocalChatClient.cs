using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

namespace Cellm.Models.Providers.FoundryLocal;

internal class FoundryLocalChatClient(string modelAlias, FoundryLocalModelManager modelManager, HttpClient httpClient) : IChatClient
{
    private IChatClient? _innerClient;

    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> chatMessages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureReadyAsync(cancellationToken);
        return await _innerClient!.GetResponseAsync(chatMessages, options, cancellationToken);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> chatMessages,
        ChatOptions? options = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureReadyAsync(cancellationToken);

        await foreach (var update in _innerClient!.GetStreamingResponseAsync(chatMessages, options, cancellationToken))
        {
            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return _innerClient?.GetService(serviceType, serviceKey);
    }

    public void Dispose()
    {
        _innerClient?.Dispose();
    }

    private async Task EnsureReadyAsync(CancellationToken cancellationToken)
    {
        if (_innerClient is not null)
        {
            return;
        }

        await modelManager.EnsureModelReadyAsync(modelAlias, cancellationToken);

        _innerClient = CreateInnerClient();
    }

    private IChatClient GetInnerClient()
    {
        _innerClient ??= CreateInnerClient();
        return _innerClient;
    }

    private IChatClient CreateInnerClient()
    {
        var baseAddress = modelManager.GetBaseAddress();

        var openAiClient = new OpenAIClient(
            new ApiKeyCredential("foundry-local"),
            new OpenAIClientOptions
            {
                Transport = new System.ClientModel.Primitives.HttpClientPipelineTransport(httpClient),
                Endpoint = baseAddress
            });

        return openAiClient.GetChatClient(modelAlias).AsIChatClient();
    }
}
