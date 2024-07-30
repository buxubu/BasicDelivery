using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class ChangeOrderViewModel
    {
        public int OrderId { get; set; }
        public int Status { get; set; }
        public string? Location { get; set; }
        public string? Reason { get; set; }
    }
}
