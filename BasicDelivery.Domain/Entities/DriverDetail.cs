using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class DriverDetail
    {
        public int DriverDetailId { get; set; }
        public int DriverId { get; set; }
        public string LicenseNumber { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string Font { get; set; } = null!;
        public string Back { get; set; } = null!;

        public virtual Driver Driver { get; set; } = null!;
    }
}
