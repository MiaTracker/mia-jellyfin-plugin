using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Entities;
using Mia.Plugin.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mia.Plugin;

public class Setup : IHostedService, IDisposable
{
    private ILibraryManager _libraryManager;
    private ILogger<Setup> _logger;
    private MiaApi _api;

    public Setup(ILibraryManager libraryManager, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
    {
        _libraryManager = libraryManager;
        _logger = loggerFactory.CreateLogger<Setup>();
        _api = new MiaApi(httpClientFactory, loggerFactory.CreateLogger<MiaApi>());
    }


    public Task StartAsync(CancellationToken token)
    {
        _libraryManager.ItemAdded += OnItemAdded;
        _libraryManager.ItemUpdated += OnItemUpdated;
        _libraryManager.ItemRemoved += OnItemRemoved;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    private void OnItemAdded(object? sender, ItemChangeEventArgs args)
    {
        var tmdbId = args.Item.GetProviderId(MetadataProvider.Tmdb);
        _logger.LogCritical($"Update: ExtenrnalId: {tmdbId}");
        if (tmdbId == null) return;
        if (!Int32.TryParse(tmdbId, out int id))
        {
            _logger.LogCritical("Failed to parse TmdbId to Int");
            return;
        }

        if (args.Item is Movie)
        {

            _api.CreateMovie(id);

        } else if (args.Item is Series)
        {
            _api.CreateSeries(id);
        }
    }

    private void OnItemUpdated(object? sender, ItemChangeEventArgs args)
    {
        var tmdbId = args.Item.GetProviderId(MetadataProvider.Tmdb);
        _logger.LogCritical($"Update: ExtenrnalId: {tmdbId}");
        if (tmdbId == null) return;
        if (!Int32.TryParse(tmdbId, out int id))
        {
            _logger.LogCritical("Failed to parse TmdbId to Int");
            return;
        }

        if (args.Item is Movie)
        {

            _api.CreateMovie(id);

        } else if (args.Item is Series)
        {
            _api.CreateSeries(id);
        }
    }

    private void OnItemRemoved(object? sender, ItemChangeEventArgs args)
    {
        var tmdbId = args.Item.GetProviderId(MetadataProvider.Tmdb);
        _logger.LogCritical($"Update: ExtenrnalId: {tmdbId}");
        if (tmdbId == null) return;
        if (!Int32.TryParse(tmdbId, out int id))
        {
            _logger.LogCritical("Failed to parse TmdbId to Int");
            return;
        }

        if (args.Item is Movie)
        {

            _api.DeleteMovie(id);

        } else if (args.Item is Series)
        {
            _api.DeleteSeries(id);
        }
    }

    public void Dispose()
    {
        _libraryManager.ItemAdded -= OnItemAdded;
    }
}