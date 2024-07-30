using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class History
    {
        public int HistoryId { get; set; }
        public int OrderId { get; set; }
        public int? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? DriverId { get; set; }
        public string? Reason { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
