using Domain.Entities;

namespace Infrastructure.Interface.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Update(Product obj);
    }
}
