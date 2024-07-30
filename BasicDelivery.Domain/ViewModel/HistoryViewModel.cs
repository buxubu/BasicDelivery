using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class HistoryViewModel
    {
        public int HistoryId { get; set; }
        public int OrderId { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverAddress { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public int? TotalMoneyOrder { get; set; }
        public string? PaymentString { get; set; }
        public int? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string? StatusDetail { get; set; }   
        public int? DriverId { get; set; }
        public string? DriverName { get; set; } 
        public string? Reason { get; set; }
    }
}
