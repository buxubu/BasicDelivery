using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Data.Abstract
{
    public interface IRepository<T> where T : class
    {
        ///<summary>
        ///
        ///</summary>
        ///<param name = "entity"></param>
        Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> condition = null);
        Task Insert(T entity);
        Task InsertMutiple(IEnumerable<T> entities);
        Task Commit();
        void Delete(T entity);
        Task DeleteMutiple(Expression<Func<T, bool>> expression);
        Task<T> GetById(int? id);
        void Update(T entity);
        Task<T> GetDetailByIdInclu(Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> include = null);
        //Task<IEnumerable<T>> GetListTInclu(Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> include = null);
        Task<IEnumerable<T>> GetListT(Expression<Func<T, bool>> condition = null);
        //Task<T> GetByIdInObject(Expression<Func<T, bool>> id);
        Task<IEnumerable<T>> GetByIdInObject(Expression<Func<T, bool>> id);
        Task<IEnumerable<T>> GetListTInclu(Expression<Func<T, bool>> conditionId = null, Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> include = null);
    }
}
