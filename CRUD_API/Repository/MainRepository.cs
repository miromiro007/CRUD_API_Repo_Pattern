using CRUD_API.data;
using CRUD_API.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRUD_API.Repository
{
    public class MainRepository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext _context; 

        public MainRepository(AppDbContext context)
        {
            _context = context;
        }


        public T SelectOne(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public IEnumerable<T> SelectMany(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task Delete(IEnumerable<T> ienumerable)
        {
            _context.Set<T>().RemoveRange(ienumerable);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var categories = await _context.Set<T>().ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }
        public Task<T> TfindByIdAsync(int id)
        {
            return _context.Set<T>().FindAsync(id).AsTask();
        }

        public Task Update(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        Task IRepository<T>.Add(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return Task.CompletedTask;
        }

        public T TfindById(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
