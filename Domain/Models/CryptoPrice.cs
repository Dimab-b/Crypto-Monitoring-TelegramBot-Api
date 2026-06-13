using System.Text.Json.Serialization;

namespace ApiWithOtherApi.Domain.Models
{
    public class CryptoPrice
    {

        public Dictionary<string, CurrencyData> Data { get; set; }
    }

    public class CurrencyData
    {
        [JsonPropertyName("usd")]
        public decimal Usd { get; set; }

    }
}