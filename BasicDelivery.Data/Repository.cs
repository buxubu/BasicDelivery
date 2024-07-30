using BasicDelivery.Data.Abstract;
using BasicDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly deliveryDbContext _deliveryDbContext;
        public Repository(deliveryDbContext deliveryDbContext)
        {
            _deliveryDbContext = deliveryDbContext;
        }

        public async Task<IEnumerable<T>> GetListT(Expression<Func<T, bool>> condition = null)
        {
            if (condition != null)
            {
                return await _deliveryDbContext.Set<T>().Where(condition).ToListAsync();
            }
            return await _deliveryDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int? id)
        {
            var value = await _deliveryDbContext.Set<T>().FindAsync(id);
            return value;
        }

        public async Task<IEnumerable<T>> GetByIdInObject(Expression<Func<T, bool>> id)
        {
            var value = await _deliveryDbContext.Set<T>().Where(id).ToListAsync();
            return value;
        }

        public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> condition = null)
        {
            return await _deliveryDbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task DeleteMutiple(Expression<Func<T, bool>> expression)
        {
            var entities = await _deliveryDbContext.Set<T>().Where(expression).ToListAsync();
            if (entities.Count > 0)
            {
                _deliveryDbContext.Set<T>().RemoveRange(entities);
            }
        }
        public void Delete(T entity)
        {
            EntityEntry entityEntry = _deliveryDbContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
        public async Task Insert(T entity)
        {
            await _deliveryDbContext.Set<T>().AddAsync(entity);
        }

        public async Task InsertMutiple(IEnumerable<T> entities)
        {
            await _deliveryDbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> GetListTInclu(Expression<Func<T, bool>> conditionId = null, Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> include = null)
        {
            if (conditionId != null && condition != null)
            {
                return await _deliveryDbContext.Set<T>().AsNoTracking().Where(conditionId).Where(condition).Include(include).ToListAsync();
            }
            else if (conditionId != null && condition == null)
            {
                return await _deliveryDbContext.Set<T>().AsNoTracking().Where(conditionId).Include(include).ToListAsync();
            }
            return await _deliveryDbContext.Set<T>().AsNoTracking().Where(condition).Include(include).ToListAsync();

        }

        public async Task<T> GetDetailByIdInclu(Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> include = null)
        {
            return await _deliveryDbContext.Set<T>().Where(condition).Include(include).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            EntityEntry entityEntry = _deliveryDbContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task Commit()
        {
            await _deliveryDbContext.SaveChangesAsync();
        }
    }
}
