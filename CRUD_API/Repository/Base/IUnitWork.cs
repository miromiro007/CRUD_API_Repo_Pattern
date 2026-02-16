using CRUD_API.Models;

namespace CRUD_API.Repository.Base
{
    public interface IUnitWork : IDisposable
    {
        IRepository<Item> Items { get; }
        IRepository<Category> Categories { get; }
        int CommitChange();
        Task<int> CommitChangeAsync();
    }
}
