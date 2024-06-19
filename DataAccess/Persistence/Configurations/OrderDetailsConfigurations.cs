using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderDetailsConfigurations : IEntityTypeConfiguration<OrderDetail>

    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne(x => x.Order)
                   .WithMany(x => x.OrderDetails)
                   .HasForeignKey(x => x.OrderID);

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.OrderDetail)
                   .HasForeignKey(x => x.ProductID);
        }
    }
}
