using System.Linq.Expressions;

namespace CRUD_API.Repository.Base
{
    public interface IRepository<T> where T : class
    {


        T SelectOne(Expression< Func<T, bool>> predicate);
        IEnumerable<T> SelectMany(Expression<Func<T, bool>> predicate);
        Task<T> TfindByIdAsync(int id);

        T TfindById(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(params string[] includes) ;

        Task AddAsync(T entity);


        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task Add(IEnumerable<T> entities);

        Task Update(IEnumerable<T> entities);

        Task Delete(IEnumerable<T> ienumerable);

    }
}
