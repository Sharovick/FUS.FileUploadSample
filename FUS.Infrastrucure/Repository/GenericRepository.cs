namespace FUS.Infrastrucure.Repository
{
    using FUS.Core.EFCore;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly FUSContext _context;
        private readonly DbSet<TEntity> _table;
        public GenericRepository(FUSContext context)
        {
            _context = context;
            _table = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _table.AsNoTracking();
        }

        public TEntity GetById(object id)
        {
            return _table.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _table.FindAsync(id);
        }

        public void Insert(TEntity obj)
        {
            _table.Add(obj);
        }

        public void Update(TEntity obj)
        {
            _table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            TEntity existing = _table.Find(id);
            _table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> obj)
        {
            await _table.AddRangeAsync(obj);
        }

        public async Task<TEntity> InsertAsync(TEntity obj)
        {
            _table.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }

}
