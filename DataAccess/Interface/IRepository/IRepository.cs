using System.Linq.Expressions;

namespace Infrastructure.Interface.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll(string? includeProperties = null);
        Task<T> Get(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
