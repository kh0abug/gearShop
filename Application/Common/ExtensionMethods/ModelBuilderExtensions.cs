using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
               new User()
               {
                   Id = 1,
                   Name = "Kien",
                   UserName = "kien1004",
                   Email = "kienltqe170124@fpt.edu.vn",
                   Password = "123",
                   Address = "Quy Nhon",
                   RegisterDate = DateTime.Now,
                   RoleID = 1
               },
               new User()
               {
                   Id = 2,
                   Name = "Vo Cong Huy",
                   UserName = "huy",
                   Email = "kiencvqe1@fpt.edu.vn",
                   Password = "123456789",
                   Address = "Quy Nhon",
                   RegisterDate = DateTime.Now,
                   RoleID = 2
               });
            modelBuilder.Entity<Role>().HasData(
              new Role() { Id = 1, RoleName = "Admin", IsAdmin = true },
              new Role() { Id = 2, RoleName = "Users", IsAdmin = false }
              );
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Mouse" },
                new Category() { Id = 2, Name = "Key Board" }
                );
            modelBuilder.Entity<Product>().HasData(
               new Product()
               {
                   Id = 1,
                   Name = "ASUS ROG Azoth 75% Wireless DIY Custom Gaming Keyboard",
                   Price = 229,
                   Stock = 200,
                   Description = "Unique gasket mount design: Silicone gasket mount with three layers of dampening foams combine to provide an unrivaled typing experience",
                   CategoryId = 2,
                   ImgUrl = ""
               },
               new Product()
               {
                   Id = 2,
                   Name = "Razer Ornata V3 X Gaming Keyboard",
                   Price = 34.99,
                   Stock = 200,
                   Description = "Low-Profile Keys: With slimmer keycaps and shorter switches, enjoy natural hand positioning that allows for long hours of use with little strain",
                   CategoryId = 2,
                   ImgUrl = ""
               },
               new Product()
               {
                   Id = 3,
                   Name = "Low-Profile Keys: With slimmer keycaps and shorter switches, enjoy natural hand positioning that allows for long hours of use with little strain",
                   Price = 129.99,
                   Stock = 200,
                   Description = "Low-Profile Keys: With slimmer keycaps and shorter switches, enjoy natural hand positioning that allows for long hours of use with little strain",
                   CategoryId = 2,
                   ImgUrl = ""
               },
               new Product()
               {
                   Id = 4,
                   Name = "Logitech G413 TKL SE Mechanical Gaming Keyboard",
                   Price = 120,
                   Stock = 200,
                   Description = "Take your gaming skills to the next level: The Logitech G413 TKL SE is a tenkeyless keyboard with gaming-first features and the durability and performance necessary to compete",
                   CategoryId = 2,
                   ImgUrl = ""
               }
               );
        }


    }
}
