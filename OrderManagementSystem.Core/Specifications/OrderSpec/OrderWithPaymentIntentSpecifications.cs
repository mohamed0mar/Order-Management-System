using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Specifications.OrderSpec
{
	public class OrderWithPaymentIntentSpecifications:BaseSpecifications<Order>
	{
		public OrderWithPaymentIntentSpecifications(string? paymentIntentId)
			: base(O => O.PaymentIntentId == paymentIntentId)
		{

		}
	}
}
