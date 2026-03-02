using TCGDex;

// Create client with default language (English)
var client = new TCGDexClient(SupportedLanguages.En);

Console.WriteLine("TCGDex .NET SDK - Initial demo");
Console.WriteLine("Base URL: {0}", client.EndpointUrl);
Console.WriteLine("Language: {0}", client.Lang.ToApiString());
Console.WriteLine();

// List series
Console.WriteLine("Fetching series...");
var series = await client.FetchSeriesAsync();
Console.WriteLine("Found {0} series.", series?.Count ?? 0);
if (series is { Count: > 0 })
{
    foreach (var s in series.Take(3))
    {
        Console.WriteLine("  - {0} (id: {1})", s.Name, s.Id);
    }
}
Console.WriteLine();

// List sets (first page)
Console.WriteLine("Fetching sets...");
var sets = await client.FetchSetsAsync();
Console.WriteLine("Found {0} sets.", sets?.Count ?? 0);
if (sets is { Count: > 0 })
{
    foreach (var set in sets.Take(3))
    {
        Console.WriteLine("  - {0} (id: {1}, cards: {2})", set.Name, set.Id, set.CardCount?.Total ?? 0);
    }
}
Console.WriteLine();

// Fetch a single card by id (example: first set, first card)
if (sets is { Count: > 0 })
{
    var firstSetId = sets[0].Id;
    var setDetail = await client.FetchSetAsync(firstSetId);
    if (setDetail?.Cards is { Count: > 0 } cards)
    {
        var firstCard = cards[0];
        var fullCard = await client.FetchCardAsync(firstCard.LocalId, firstSetId);
        if (fullCard != null)
        {
            Console.WriteLine("Sample card: {0}", fullCard.Name);
            Console.WriteLine("  Category: {0}, Rarity: {1}", fullCard.Category, fullCard.Rarity);
            Console.WriteLine("  Image URL: {0}", fullCard.GetImageUrl());
        }
    }
}

Console.WriteLine();
Console.WriteLine("Demo completed.");
