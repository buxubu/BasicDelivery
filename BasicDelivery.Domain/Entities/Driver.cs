using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class Driver
    {
        public Driver()
        {
            DriverDetails = new HashSet<DriverDetail>();
            Orders = new HashSet<Order>();
        }

        public int DriverId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? ReviewRate { get; set; }
        public string? Role { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<DriverDetail> DriverDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
