using System.Text.Json.Serialization;

namespace TheBooksNook.Models;

public class VolumeInfo
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("authors")]
    public List<string> Authors { get; set; } = [];

    [JsonPropertyName("publisher")]
    public string Publisher { get; set; } = "";

    [JsonPropertyName("publishedDate")]
    public string PublishedDate { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = [];
}
