using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace GearShopWeb.Areas.Common
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        public AccountService(ApplicationDbContext db)
        {
            _db = db;
        }
        public User Login(string username, string password)
        {
            var user = _db.Users.Include(u => u.Role).AsNoTracking().SingleOrDefault(x =>
                       x.UserName.Equals(username) &&
                       x.Password.Equals(password.MD5Hash()));
            return user;
        }

        public void SignUp(string name, string username, string email, string password)
        {
            var newUser = new User();
            newUser.Name = name;
            newUser.Email = email;
            newUser.Password = password.MD5Hash();
            newUser.UserName = username;
            newUser.RoleID = 2;
            _db.Users.Add(newUser);
            _db.SaveChanges();
        }

    }
}
