using System.Text.Json.Serialization;

namespace TheBooksNook.Models;

public class BookApi
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    public VolumeInfo? VolumeInfo { get; set; }
}
