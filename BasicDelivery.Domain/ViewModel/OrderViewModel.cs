using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string? UserPhone { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? UserAddress { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ReceiverAddress")]
        [StringLength(300, ErrorMessage = "Length must be 300")]
        public string ReceiverAddress { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ReceiverDistrict")]
        [StringLength(300, ErrorMessage = "Length must be 300")]
        public string ReceiverDistrict { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ReceiverWard")]
        [StringLength(300, ErrorMessage = "Length must be 300")]
        public string ReceiverWard { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ReceiverName")]
        [StringLength(100, ErrorMessage = "Length must be 100")]
        public string ReceiverName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ReceiverPhone")]
        [StringLength(12, ErrorMessage = "Length must be 300")]
        public string ReceiverPhone { get; set; } = null!;
        public int? ShipCost { get; set; }
        public int? TotalMoney { get; set; }
        public bool? PaymentMethod { get; set; }
        public string? PaymentString { get; set; }
        public int? Status { get; set; }
        public string? StatusString { get; set; }
        public string? Location { get; set; }
        public DateTime? DriverAcceptAt { get; set; }
        public DateTime? CompleteAt { get; set; }
        public string? UserNote { get; set; }
        public string? DeliveryNote { get; set; }
        public int? TotalGamPackage { get; set; }
        public string? ImagesPackages { get; set; }
        public IFormFile? UploadImagesPackage { get; set; }
        public int? WidePackage { get; set; } = 10;
        public int? HeightPackage { get; set; } = 10;
        public int? LongPackage { get; set; } = 10;
        public int? TotalPriceProduct { get; set; }
        public int? TotalCod { get; set; }
        public bool? Active { get; set; }
        public int? DriverId { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int? FailedDeliveryMoney { get; set; }
        public bool? RequestSeeProduct { get; set; }
        public string? StatusDetail { get; set; }   

        public IEnumerable<OrderDetailViewModel>? ListOrderDetail { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImagesProduct { get; set; }
        public IFormFile? UploadImagesProduct { get; set; }
        public int Gam { get; set; }
        public int Quantity { get; set; }
    }
}
