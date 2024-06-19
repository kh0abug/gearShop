using Domain.Entities;

namespace Infrastructure.Interface.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order obj);
    }
}
