using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string CodeProduct { get; set; } = null!;
        public string NameProduct { get; set; } = null!;
        public string? ImagesProduct { get; set; }
        public int ProductVolume { get; set; }
        public int Price { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
