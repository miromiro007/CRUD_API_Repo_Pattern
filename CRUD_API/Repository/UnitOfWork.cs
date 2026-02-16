using CRUD_API.data;
using CRUD_API.Models;
using CRUD_API.Repository.Base;

namespace CRUD_API.Repository
{
    public class UnitOfWork : IUnitWork
    {
        private readonly AppDbContext _context;

        public IRepository<Item> Items { get; private set; }

        public IRepository<Category> Categories { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Items = new MainRepository<Item>(_context);
            Categories = new MainRepository<Category>(_context);
        }
        public int CommitChange()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
