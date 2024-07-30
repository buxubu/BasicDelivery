using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class Order
    {
        public Order()
        {
            Histories = new HashSet<History>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string ReceiverAddress { get; set; } = null!;
        public string ReceiverDistrict { get; set; } = null!;
        public string ReceiverWard { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public int? ShipCost { get; set; }
        public int? TotalMoney { get; set; }
        public bool? PaymentMethod { get; set; }
        public int Status { get; set; }
        public string? Location { get; set; }
        public DateTime? DriverAcceptAt { get; set; }
        public DateTime? CompleteAt { get; set; }
        public string? UserNote { get; set; }
        public string? DeliveryNote { get; set; }
        public int? TotalGamPackage { get; set; }
        public string? ImagesPackages { get; set; }
        public int WidePackage { get; set; }
        public int HeightPackage { get; set; }
        public int LongPackage { get; set; }
        public int? TotalPriceProduct { get; set; }
        public int? TotalCod { get; set; }
        public bool? Active { get; set; }
        public int? DriverId { get; set; }
        public string UserPhone { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserAddress { get; set; } = null!;
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int? FailedDeliveryMoney { get; set; }
        public bool? RequestSeeProduct { get; set; }
        public string? StatusDetail { get; set; }

        public virtual Driver? Driver { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
