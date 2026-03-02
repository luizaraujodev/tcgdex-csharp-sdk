using TCGDex;
using Xunit;

namespace TCGDex.Tests;

public class TCGDexClientTests
{
    [Fact]
    public void Client_Constructor_SetsDefaultLanguage()
    {
        var client = new TCGDexClient();
        Assert.Equal(SupportedLanguages.En, client.Lang);
        Assert.Equal("en", client.Lang.ToApiString());
    }

    [Fact]
    public void Client_Constructor_AcceptsLanguage()
    {
        var client = new TCGDexClient(SupportedLanguages.Pt);
        Assert.Equal(SupportedLanguages.Pt, client.Lang);
        Assert.Equal("pt", client.Lang.ToApiString());
    }

    [Fact]
    public void ApiEndpoints_IsValid_AcceptsKnownEndpoints()
    {
        Assert.True(ApiEndpoints.IsValid("cards"));
        Assert.True(ApiEndpoints.IsValid("sets"));
        Assert.True(ApiEndpoints.IsValid("series"));
        Assert.True(ApiEndpoints.IsValid("random"));
        Assert.False(ApiEndpoints.IsValid("unknown"));
    }

    [Fact]
    public void Query_Create_ReturnsNewQuery()
    {
        var q = Query.Create();
        Assert.NotNull(q);
        Assert.Empty(q.Params);
    }

    [Fact]
    public void Query_Equal_AddsParam()
    {
        var q = Query.Create().Equal("name", "Pikachu");
        Assert.Single(q.Params);
        Assert.Equal("name", q.Params[0].Key);
        Assert.Equal("eq:Pikachu", q.Params[0].Value.ToString());
    }

    [Fact]
    public async Task Client_FetchSeriesAsync_ReturnsList()
    {
        var client = new TCGDexClient(SupportedLanguages.En);
        var series = await client.FetchSeriesAsync();
        Assert.NotNull(series);
        Assert.NotEmpty(series);
    }

    [Fact]
    public async Task Client_FetchSetAsync_ReturnsSet()
    {
        var client = new TCGDexClient(SupportedLanguages.En);
        var sets = await client.FetchSetsAsync();
        Assert.NotNull(sets);
        Assert.NotEmpty(sets);
        var set = await client.FetchSetAsync(sets![0].Id);
        Assert.NotNull(set);
        Assert.Equal(sets[0].Id, set.Id);
        Assert.NotNull(set.Cards);
    }
}
