using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystem.Core.Entities.Order;

namespace OrderManagementSystem.Repository._Data.Config
{
	internal class CustomerConfigurations : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.HasMany(c => c.Orders)
				.WithOne();
		}
	}
}
