using Domain.Entities;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Product obj)
        {
            var product = await _db.Products.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if (product != null)
            {
                product.Name = obj.Name;
                product.Description = obj.Description;
                product.Price = obj.Price;
                product.Stock = obj.Stock;
                product.CategoryId = obj.CategoryId;
                if (product.ImgUrl != null)
                {
                    product.ImgUrl = obj.ImgUrl;
                }
            }

        }
    }
}
