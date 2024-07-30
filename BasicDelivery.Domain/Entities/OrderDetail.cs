using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string? ImagesProduct { get; set; }
        public int? Gam { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product? Product { get; set; }
    }
}
