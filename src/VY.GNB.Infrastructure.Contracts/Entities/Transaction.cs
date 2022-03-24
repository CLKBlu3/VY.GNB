using System;
using System.Collections.Generic;

#nullable disable

namespace VY.GNB.Infrastructure.Contracts.Entities
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
}
