namespace Infrastructure.Interface.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
        IOrderRepository Order { get; }
        Task Save();
    }
}
