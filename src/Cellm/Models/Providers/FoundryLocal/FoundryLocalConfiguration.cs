using Microsoft.Extensions.AI;

namespace Cellm.Models.Providers.FoundryLocal;

internal class FoundryLocalConfiguration : IProviderConfiguration
{
    public Provider Id { get => Provider.FoundryLocal; }

    public string Name { get => "Foundry Local"; }

    public string Icon { get => $"AddIn/UserInterface/Resources/{nameof(Provider.FoundryLocal)}.png"; }

    public string DefaultModel { get; init; } = string.Empty;

    public string SmallModel { get; init; } = string.Empty;

    public string MediumModel { get; init; } = string.Empty;

    public string LargeModel { get; init; } = string.Empty;

    public AdditionalPropertiesDictionary? AdditionalProperties { get; init; } = [];

    public bool SupportsJsonSchemaResponses { get; init; } = false;

    public bool SupportsStructuredOutputWithTools { get; init; } = false;

    public bool IsEnabled { get; init; } = true;
}
