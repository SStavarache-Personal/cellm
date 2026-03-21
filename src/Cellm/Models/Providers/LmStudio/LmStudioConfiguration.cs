using Microsoft.Extensions.AI;

namespace Cellm.Models.Providers.LmStudio;

internal class LmStudioConfiguration : IProviderConfiguration
{
    public Provider Id { get => Provider.LmStudio; }

    public string Name { get => "LM Studio"; }

    public string Icon { get => $"AddIn/UserInterface/Resources/{nameof(Provider.LmStudio)}.png"; }

    public Uri BaseAddress { get; init; } = new Uri("http://localhost:1234/v1/");

    public string DefaultModel { get; init; } = string.Empty;

    public string ApiKey { get; init; } = "lm-studio";

    public int HttpTimeoutInSeconds = 3600;

    public string SmallModel { get; init; } = string.Empty;

    public string MediumModel { get; init; } = string.Empty;

    public string LargeModel { get; init; } = string.Empty;

    public AdditionalPropertiesDictionary? AdditionalProperties { get; init; } = [];

    public bool SupportsJsonSchemaResponses { get; init; } = false;

    public bool SupportsStructuredOutputWithTools { get; init; } = true;

    public bool IsEnabled { get; init; } = true;
}
