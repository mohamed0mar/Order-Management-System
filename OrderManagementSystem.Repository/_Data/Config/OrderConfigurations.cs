using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository._Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(O => O.TotalAmount)
				.HasColumnType("decimal(18,2)");

			builder.Property(O => O.Status)
				.HasConversion(
				(OStatus) => OStatus.ToString(),
				(OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
				);
			builder.Property(O => O.PaymentMethod)
				.HasConversion(
				(Pay) => Pay.ToString(),
				(pay) => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), pay)
				);
		



		}
	}
}
