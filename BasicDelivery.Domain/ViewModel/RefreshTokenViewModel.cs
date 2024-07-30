using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class RefreshTokenViewModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
