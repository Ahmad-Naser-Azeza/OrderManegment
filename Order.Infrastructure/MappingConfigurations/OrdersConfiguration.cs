
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace CoreOps.FleetManagment.Infrastructure.MappingConfigurations;

public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
         builder.ToTable("Orders");
         builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
