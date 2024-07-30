using BasicDelivery.Data.Abstract;
using BasicDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Service.DriverTokenService
{
    public class DriverTokenService : IDriverTokenService
    {
        private readonly IRepository<DriverToken> _driverTokenReponsitory;
        public DriverTokenService(IRepository<DriverToken> driverTokenReponsitory)
        {
            _driverTokenReponsitory = driverTokenReponsitory;
        }

        public async Task SaveToken(DriverToken model)
        {
            await _driverTokenReponsitory.Insert(model);
            await _driverTokenReponsitory.Commit();
        }

        public async Task<DriverToken> FindSerialNumber(string serialNmber)
        {
            return await _driverTokenReponsitory.GetSingleByConditionAsync(x => x.CodeRefreshToken == serialNmber);
        }
    }
}
