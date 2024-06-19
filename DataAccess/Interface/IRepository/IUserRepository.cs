using Domain.Entities;

namespace Infrastructure.Interface.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User obj);
    }
}
