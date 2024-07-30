using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BasicDelivery.ModelView
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        //[Required]
        public string? FullName { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        //[StringLength(300, ErrorMessage = "Length must be 300")]
        //[EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? UploadAvatar { get; set; }
        //[Required]
        public string? Address { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        //[StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public string? Role { get; set; }
        public bool? Active { get; set; }
        public string? Phone { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? CreateDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? LastLogin { get; set; }
    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        [StringLength(300, ErrorMessage = "Length must be 300")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        public string? PasswordHash { get; set; }
    }
}
