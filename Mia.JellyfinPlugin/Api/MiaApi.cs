using System.Net.Mime;
using System.Text;
using System.Text.Json;
using MediaBrowser.Common.Net;
using Mia.Plugin.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Mia.Plugin.Api;

public class MiaApi
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MiaApi> _logger;

    public MiaApi(IHttpClientFactory httpClientFactory, ILogger<MiaApi> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task CreateMovie(int tmdbId)
    {
        var model = new MediaCreate()
        {
            TMDBId = tmdbId,
            Source = new SourceCreate()
            {
                Name = Plugin.Instance?.Configuration.SourceName ?? "",
                Type = Plugin.Instance?.Configuration.SourceType ?? "",
                Url = Plugin.Instance?.Configuration.SourceUrl ?? ""
            }
        };

        await Post("/movies/source_create", data: model);
    }

    public async Task CreateSeries(int tmdbId)
    {
        var model = new MediaCreate()
        {
            TMDBId = tmdbId,
            Source = new SourceCreate()
            {
                Name = Plugin.Instance?.Configuration.SourceName ?? "",
                Type = Plugin.Instance?.Configuration.SourceType ?? "",
                Url = Plugin.Instance?.Configuration.SourceUrl ?? ""
            }
        };

        await Post("/series/source_create", data: model);
    }

    public async Task DeleteMovie(int tmdbId)
    {
        var model = new MediaDelete()
        {
            TMDBId = tmdbId,
            Source = Plugin.Instance?.Configuration.SourceName ?? ""
        };

        await Post("/movies/source_delete", data: model);
    }

    public async Task DeleteSeries(int tmdbId)
    {
        var model = new MediaDelete()
        {
            TMDBId = tmdbId,
            Source = Plugin.Instance?.Configuration.SourceName ?? ""
        };

        await Post("/series/source_delete", data: model);
    }

    private async Task Post(string url, Dictionary<String, object>? parameters = null, object? data = null)
    {
        if (String.IsNullOrEmpty(Plugin.Instance?.Configuration.BaseUrl) ||
            String.IsNullOrEmpty(Plugin.Instance?.Configuration.Token))
        {
            _logger.LogCritical("BaseUrl or Token not set. Cannot sync data with Mia instance!");
            return;
        }

        var httpClient = GetHttpClient();
        var u = BuildUrl(url, parameters);

        _logger.LogCritical($"Url: {u}");

        var x = JsonSerializer.Serialize(data);
        _logger.LogCritical($"Data: {x}");

        var json = JsonSerializer.SerializeToUtf8Bytes(data);
        using var content = new ByteArrayContent(json);
        content.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);

        try
        {
            var res = await httpClient.PostAsync(u, content);
            _logger.LogCritical($"Response: {res.StatusCode}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    private HttpClient GetHttpClient()
    {
        var client = _httpClientFactory.CreateClient(NamedClient.Default);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Plugin.Instance?.Configuration.Token}");
        return client;
    }

    private string BuildUrl(string url, Dictionary<String, object>? parameters)
    {
        var builder = new StringBuilder();

        builder.Append(Plugin.Instance?.Configuration.BaseUrl);

        builder.Append(url);

        if (parameters != null && parameters.Count > 0)
        {
            builder.Append("?");
            bool first = true;
            foreach (var (key, val) in parameters)
            {
                if (!first) builder.Append("&");
                else first = false;

                builder.Append($"{key}={val}");
            }
        }

        return builder.ToString();
    }
}