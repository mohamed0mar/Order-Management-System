using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystem.Core.Entities.Order;

namespace OrderManagementSystem.Repository._Data.Config
{
	internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.Property(Oi => Oi.UnitPrice)
				.HasColumnType("decimal(18,2)");

			builder.Property(Oi => Oi.Discount)
			.HasColumnType("decimal(18,2)");

			
		}
	}
}
