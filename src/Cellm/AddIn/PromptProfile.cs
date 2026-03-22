namespace Cellm.AddIn;

public class PromptProfile
{
    public string Name { get; init; } = string.Empty;

    public string SystemPrompt { get; init; } = string.Empty;

    public double Temperature { get; init; }

    public int MaxOutputTokens { get; init; } = 8192;

    public ThinkingLevel ThinkingLevel { get; init; } = ThinkingLevel.Off;
}
