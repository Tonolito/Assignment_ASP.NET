using System.Text.Json.Serialization;
namespace Domain.Dtos;

public class CookieConsentDto
{
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("functional")]

    public bool Functional { get; set; }
}
