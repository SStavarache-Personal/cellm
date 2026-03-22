using Cellm.AddIn;
using Cellm.Models.Prompts;
using Microsoft.Extensions.Options;

namespace Cellm.Models.Providers.Behaviors;

internal class ThinkingBehavior(IOptionsMonitor<CellmAddInConfiguration> cellmAddInConfiguration) : IProviderBehavior
{
    public bool IsEnabled(Provider provider)
    {
        return true;
    }

    public void Before(Provider provider, Prompt prompt)
    {
        var thinkingLevel = cellmAddInConfiguration.CurrentValue.ThinkingLevel;

        if (thinkingLevel == ThinkingLevel.Off)
        {
            return;
        }

        prompt.Options.AdditionalProperties ??= [];

        switch (provider)
        {
            case Provider.LmStudio:
            case Provider.OpenAiCompatible:
                prompt.Options.AdditionalProperties["enable_thinking"] = true;
                prompt.Options.AdditionalProperties["thinking_budget"] = GetTokenBudget(thinkingLevel);
                break;

            case Provider.OpenAi:
            case Provider.OpenRouter:
                prompt.Options.AdditionalProperties["reasoning_effort"] = thinkingLevel switch
                {
                    ThinkingLevel.Low => "low",
                    ThinkingLevel.Medium => "medium",
                    ThinkingLevel.High => "high",
                    _ => "medium"
                };
                break;

            case Provider.Gemini:
                prompt.Options.AdditionalProperties["thinking_budget"] = GetTokenBudget(thinkingLevel);
                break;

            case Provider.Ollama:
                prompt.Options.AdditionalProperties["think"] = true;
                break;

                // DeepSeek: deepseek-reasoner always reasons, no toggle needed
                // Mistral: thinking models think by default, MistralThinkingBehavior handles response
                // Anthropic: thinking is handled via the SDK's native API
                // Aws/Azure: pass-through to underlying model
        }
    }

    public void After(Provider provider, Prompt prompt) { }

    public uint Order => 25;

    private static int GetTokenBudget(ThinkingLevel level)
    {
        return level switch
        {
            ThinkingLevel.Low => 1024,
            ThinkingLevel.Medium => 4096,
            ThinkingLevel.High => 16384,
            _ => 4096
        };
    }
}
