using System.Text.Json.Serialization;

namespace Mia.Plugin.Models;

public class SourceCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
}