# TCGDex .NET SDK

.NET SDK for the [TCGDex API](https://api.tcgdex.net/v2) — a REST API for Pokemon TCG collectors.

## Requirements

- .NET 10.0 SDK

## Installation

**From NuGet** (when published):

```bash
dotnet add package TCGDex
```

Or in your `.csproj`:

```xml
<PackageReference Include="TCGDex" Version="0.1.0" />
```

**From source:**

```xml
<ProjectReference Include="path\to\TCGDex\TCGDex.csproj" />
```

## Quick Start

```csharp
using TCGDex;

// Create client (default language: English)
var client = new TCGDexClient(SupportedLanguages.En);

// List all series
var series = await client.FetchSeriesAsync();
foreach (var s in series)
    Console.WriteLine("{0} ({1})", s.Name, s.Id);

// List sets (optionally filtered by series)
var sets = await client.FetchSetsAsync();

// Get a set by id
var set = await client.FetchSetAsync("swsh12");
Console.WriteLine("Set: {0}, Cards: {1}", set?.Name, set?.Cards?.Count);

// Get a card by global id or by set + local id
var card = await client.FetchCardAsync("swsh12-1");
// or: await client.FetchCardAsync("1", "swsh12");

// Use endpoints directly
var cardById = await client.Cards.GetAsync("swsh12-1");
var setsList = await client.Sets.ListAsync(Query.Create().Paginate(1, 10));

// Random card / set / series
var randomCard = await client.Random.GetRandomCardAsync();
```

## Configuration

- **Language**: Set via constructor or `client.Lang = SupportedLanguages.Pt`.
- **Base URL**: `client.EndpointUrl = "https://api.tcgdex.net/v2"`.
- **Cache**: Replace `client.Cache` with a custom `ITCGDexCache` implementation.
- **Cache TTL**: `client.CacheTTL = 3600` (seconds).

## Query Builder

Filter and sort list results with the fluent `Query` API:

```csharp
var query = Query.Create()
    .Equal("rarity", "Rare")
    .Sort("name", SortOrder.Asc)
    .Paginate(1, 20);
var cards = await client.Cards.ListAsync(query);
```

## Project Structure

- **`src/TCGDex/`** — Main SDK library (namespace `TCGDex`).
- **`src/TCGDex.Console/`** — Sample console application.
- **`tests/TCGDex.Tests/`** — Unit and integration tests.

All interfaces use the **I** prefix (e.g. `ICard`, `ISet`, `ISerieResume`). **TCG** is always uppercase in type and namespace names.

## Creating and publishing the NuGet package

### Manual

From the repository root:

```bash
dotnet pack src/TCGDex/TCGDex.csproj -c Release -o ./nupkgs
```

This produces `nupkgs/TCGDex.0.1.0.nupkg` (and optionally a `.snupkg` for symbols). To publish to NuGet.org:

```bash
dotnet nuget push nupkgs/TCGDex.0.1.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### GitHub Actions

The workflow `.github/workflows/ci.yml` runs tests on every push and pull request. It publishes to NuGet.org when:

1. **A GitHub Release is published** — Create a release (e.g. v0.1.0) and the package will be packed and pushed automatically.
2. **Manual run** — Go to Actions → CI and Publish → Run workflow.

Add the `NUGET_API_KEY` secret in your repository settings (Settings → Secrets and variables → Actions) with your [NuGet.org API key](https://www.nuget.org/account/apikeys).

## License

See repository license.
