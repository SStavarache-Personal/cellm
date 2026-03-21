# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Cellm is a Windows Excel add-in that lets users call LLMs directly from Excel formulas via `=PROMPT()`. It supports multiple providers (OpenAI, Anthropic, LM Studio, Ollama, Google Gemini, Mistral, AWS Bedrock, Azure, DeepSeek, OpenRouter) and Model Context Protocol (MCP) servers. Built on .NET 9.0 with ExcelDna. The default provider is LM Studio (local inference).

## Build & Test Commands

```bash
dotnet restore --locked-mode    # Restore (uses packages.lock.json)
dotnet build --no-restore       # Build
dotnet test --filter "FullyQualifiedName~Cellm.Tests.Unit" --no-build  # Unit tests only
dotnet format --verify-no-changes  # Lint check (CI uses this)
dotnet format                   # Auto-fix formatting
```

Integration tests (`Cellm.Tests.Integration`) require Excel and are not run in CI.

## Versioning & Installer

The version must be kept in sync in **two** files:
- `src/Cellm/Cellm.csproj` (`<Version>`)
- `src/Cellm.Installers/Package.wxs` (`Version=`)

**Bump the version in both files** whenever producing a new installer build. The MSI uses `MajorUpgrade`, so a version bump is required to replace previously installed files.

Build the installer:
```bash
dotnet publish src/Cellm/Cellm.csproj -c Release --no-restore
dotnet build src/Cellm.Installers/Cellm.Installer.wixproj -c Release
# Output: src/Cellm.Installers/bin/Release/en-US/Cellm-AddIn-Release-.msi
```

## Architecture

### Request Pipeline (MediatR)

All LLM calls flow through a MediatR pipeline:

```
Excel formula (CellmFunctions) → ArgumentParser → PromptBuilder → Prompt
  → MediatR pipeline:
    CacheBehavior → ToolBehavior → ProviderBehavior → UsageBehavior
  → ProviderRequestHandler → ChatClientFactory → IChatClient → LLM response
```

- **`CellmFunctions`** — Excel UDF definitions (`=PROMPT()`, `=PROMPT.TOROW()`, etc.)
- **`Client`** — Orchestrates prompt construction and dispatches `ProviderRequest` via MediatR
- **`ProviderRequestHandler`** — Gets an `IChatClient` from `ChatClientFactory` and calls the LLM
- **Behaviors** — Pipeline stages for caching, tool injection, provider-specific adjustments, and usage tracking

### Key Abstractions

- **`Prompt`** — Record holding `ChatMessage` list, `ChatOptions`, and `StructuredOutputShape`
- **`PromptBuilder`** — Fluent builder for constructing `Prompt` from Excel range data
- **`IProviderConfiguration`** — Per-provider config (models, API keys, feature flags)
- **`IChatClientFactory`** — Creates `IChatClient` (from Microsoft.Extensions.AI) per provider
- **`IProviderBehavior`** — Before/After hooks for provider-specific prompt adjustments (ordered via `Order` property)
- **`PromptProfile`** — Named configuration profiles (system prompt, temperature, max tokens)

### Provider System

Each provider has its own namespace under `Models/Providers/` with a configuration class implementing `IProviderConfiguration`. Providers are registered via keyed DI and resolved by the `Provider` enum. Provider-specific quirks (temperature handling, additional properties) are handled by `IProviderBehavior` implementations in `Models/Providers/Behaviors/`.

### Tools

- **FileSearch** / **FileReader** — Built-in tools exposed as `AIFunction` to LLMs
- **MCP** — External tool servers (e.g., Playwright) configured in appsettings.json, loaded dynamically

### Configuration

Layered: `appsettings.json` → `appsettings.{DOTNET_ENVIRONMENT}.json` → `appsettings.Local.json`. Key sections: provider configs, profiles, resilience (Polly rate limiting/retry), caching, tools, MCP servers.

## Code Style

Enforced by `.editorconfig` and `dotnet format`. Key conventions:
- Nullable reference types enabled
- `internal` access for most types (only provider configs and enums are `public`)
- Modern C# patterns: file-scoped namespaces, pattern matching, range operators
- CA1416 suppressed globally (ExcelDna is Windows-only by design)

## Testing

- **xUnit** with **NSubstitute** for mocking
- `TestServices` helper provides minimal DI container for unit tests
- `MockChatClient` wraps NSubstitute's `IChatClient` for provider testing
- Test filter: `FullyQualifiedName~Cellm.Tests.Unit` (used in CI)
