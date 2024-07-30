using BasicDelivery.Data.Abstract;
using BasicDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Service.UserTokenService
{

    public class UserTokenService : IUserTokenService
    {
        private readonly IRepository<UserToken> _userTokenReponsitory;
        public UserTokenService(IRepository<UserToken> userTokenReponsitory)
        {
            _userTokenReponsitory = userTokenReponsitory;
        }

        public async Task SaveToken(UserToken model)
        {
            await _userTokenReponsitory.Insert(model);
            await _userTokenReponsitory.Commit();
        }

        public async Task<UserToken> FindSerialNumber(string serialNmber)
        {
            return await _userTokenReponsitory.GetSingleByConditionAsync(x => x.CodeRefreshToken == serialNmber);
        }
    }
}
