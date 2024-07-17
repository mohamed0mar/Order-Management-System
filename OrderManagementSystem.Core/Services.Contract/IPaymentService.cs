using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services.Contract
{
    public interface IPaymentService
	{
		Task<Basket?> CreateOrUpdatePaymentIntent(string basketId);

		Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid);
	}
}
