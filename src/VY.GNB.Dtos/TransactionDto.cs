using System.Text.Json.Serialization;

namespace VY.GNB.Dtos
{
    public class TransactionDto
    {
        [JsonPropertyName("sku")]
        public string ProductName { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

    }
}
