using Domain.Entities;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;

namespace DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
        //public void Check(User obj,string uname,string pword)
        //{
        //   var c = _db.Users.SingleOrDefault(obj.UserName.Equals(uname) && obj.Password.Equals(pword));
        //}

    }
}
