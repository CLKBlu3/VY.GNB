using System.Text.Json.Serialization;

namespace VY.GNB.Business.Implementation.Domain
{
    public class RateDomain
    {
        [JsonPropertyName("from")]
        public string From { get; set; }
        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("rate")]
        public double Ratio { get; set; }

        [JsonIgnore]
        public bool IsVisited { get; set; }
    }
}
