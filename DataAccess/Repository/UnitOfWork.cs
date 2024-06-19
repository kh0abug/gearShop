using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Readonlys 

        private readonly ApplicationDbContext _db;

        #endregion 

        #region Properties

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderRepository Order { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="db">The Database Context</param>
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            User = new UserRepository(_db);
            Order = new OrderRepository(_db);
        }

        #region Methods

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        #endregion

    }
}
