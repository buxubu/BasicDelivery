using System;
using System.Collections.Generic;

namespace BasicDelivery.Domain.Entities
{
    public partial class UserToken
    {
        public int UserTokenId { get; set; }
        public int? UserId { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? ExpiredDateAccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiredRefreshToken { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CodeRefreshToken { get; set; }
    }
}
