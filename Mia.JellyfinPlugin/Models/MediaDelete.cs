using System.Text.Json.Serialization;

namespace Mia.Plugin.Models;

public class MediaDelete
{
    [JsonPropertyName("tmdb_id")]
    public int TMDBId { get; set; }
    [JsonPropertyName("source")]
    public string Source { get; set; }
}