using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services.Contract
{
    public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string buyerEmail, string basketId,int customerId);

		Task<Order?> GetOrderByIdAsync(int orderId);


		//For Admain

		Task<IReadOnlyList<Order>> GetAllOrdersAsync();

		Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus);


	}
}
