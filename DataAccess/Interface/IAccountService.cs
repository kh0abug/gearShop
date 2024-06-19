using Domain.Entities;

namespace Infrastructure.Interface
{
    public interface IAccountService
    {
        public User Login(string username, string password);
        public void SignUp(string name, string username, string email, string password);
    }
}
