using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUS.Infrastrucure.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        void Insert(TEntity obj);
        Task<TEntity> InsertAsync(TEntity obj);
        Task InsertRangeAsync(IEnumerable<TEntity> obj);
        void Update(TEntity obj);
        void Delete(object id);
        void Save();
        Task SaveAsync();
    }
}
