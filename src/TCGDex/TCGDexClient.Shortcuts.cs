using TCGDex.Models;

namespace TCGDex;

/// <summary>
/// Shortcut fetch methods (partial).
/// </summary>
public partial class TCGDexClient
{
    /// <summary>
    /// Fetches a card by global id, or by set id + local card id.
    /// </summary>
    public async Task<CardModel?> FetchCardAsync(string id, string? setId = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(setId))
        {
            var path = new[] { "sets", setId, id };
            var json = await FetchAsync(path, null, cancellationToken).ConfigureAwait(false);
            if (json == null) return null;
            var model = new CardModel(this);
            ModelBase.Build(model, json.Value);
            return model;
        }
        return await Cards.GetAsync(id, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Fetches cards list, optionally filtered by set id.
    /// </summary>
    public async Task<IReadOnlyList<CardResumeModel>> FetchCardsAsync(string? setId = null, Query? query = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(setId))
        {
            var set = await Sets.GetAsync(setId, cancellationToken).ConfigureAwait(false);
            return set?.Cards ?? [];
        }
        return await Cards.ListAsync(query, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Fetches a set by id.
    /// </summary>
    public Task<SetModel?> FetchSetAsync(string setId, CancellationToken cancellationToken = default) =>
        Sets.GetAsync(setId, cancellationToken);

    /// <summary>
    /// Fetches sets list, optionally filtered by series id.
    /// </summary>
    public async Task<IReadOnlyList<SetResumeModel>> FetchSetsAsync(string? seriesId = null, Query? query = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(seriesId))
        {
            var series = await Series.GetAsync(seriesId, cancellationToken).ConfigureAwait(false);
            return series?.Sets ?? [];
        }
        return await Sets.ListAsync(query, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Fetches a series by id.
    /// </summary>
    public Task<SerieModel?> FetchSerieAsync(string seriesId, CancellationToken cancellationToken = default) =>
        Series.GetAsync(seriesId, cancellationToken);

    /// <summary>
    /// Fetches all series.
    /// </summary>
    public Task<IReadOnlyList<SerieResumeModel>> FetchSeriesAsync(Query? query = null, CancellationToken cancellationToken = default) =>
        Series.ListAsync(query, cancellationToken);
}
