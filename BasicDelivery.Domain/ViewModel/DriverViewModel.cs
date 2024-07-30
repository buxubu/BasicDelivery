using BasicDelivery.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class DriverViewModel : DriverDetailViewModel
    {
        public string? FullName { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        //[StringLength(300, ErrorMessage = "Length must be 300")]
        //[EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? UploadAvatar { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? ReviewRate { get; set; }
        public string? Role { get; set; }
        public string? Phone { get; set; }
    }
    public class DriverDetailViewModel
    {
        public int? DriverDetailId { get; set; }
        public int? DriverId { get; set; }
        [StringLength(maximumLength: 20,ErrorMessage = "Length must be 20")]
        public string LicenseNumber { get; set; } = null!;
        [StringLength(maximumLength: 20, ErrorMessage = "Length must be 20")]
        public string? VehicleModel { get; set; }
        public string? Font { get; set; }
        public IFormFile? UploadFont { get; set; }
        public string? Back { get; set; }
        public IFormFile? UploadBack { get; set; }

    }
}
