using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VY.GNB.Dtos
{
    public class TransactionDtoContext
    {
        public IEnumerable<TransactionDto> TransactionData { get; set; }
        public double TotalAmount { get; set; }
    }
}
