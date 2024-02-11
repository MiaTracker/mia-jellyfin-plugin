using System.Text.Json.Serialization;

namespace Mia.Plugin.Models;

public class MediaCreate
{
    [JsonPropertyName("tmdb_id")]
    public int TMDBId { get; set; }
    [JsonPropertyName("source")]
    public SourceCreate Source { get; set; }
}