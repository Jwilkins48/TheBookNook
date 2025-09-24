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

    [JsonPropertyName("imageLinks")]
    public ImageLinks? ImageLinks { get; set; }
}

public class ImageLinks
{
    [JsonPropertyName("smallThumbnail")]
    public string SmallThumbnail { get; set; } = "";

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = "";

    [JsonPropertyName("small")]
    public string Small { get; set; } = "";

    [JsonPropertyName("medium")]
    public string Medium { get; set; } = "";

    [JsonPropertyName("large")]
    public string Large { get; set; } = "";
}
