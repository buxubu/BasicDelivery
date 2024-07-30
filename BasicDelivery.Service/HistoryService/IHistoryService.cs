using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Service.HistoryService
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryViewModel>> GetHistories();
        Task InsertHistory(History model);
        Task<History> EditHistory(History model, int idHistory);
        Task<IEnumerable<HistoryViewModel>> GetHistoriesDriver();
    }
}
