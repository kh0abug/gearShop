using Domain.Entities;

namespace Infrastructure.Interface.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
