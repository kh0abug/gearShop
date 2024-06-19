using Domain.Entities;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;

namespace DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order obj)
        {
            _db.Order.Update(obj);
        }
    }
}
