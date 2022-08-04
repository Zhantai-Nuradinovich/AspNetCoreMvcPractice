using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync();
    }
}
