using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services.Contract
{
    public interface ICustomerService
	{
		Task<Customer?> CreateCustomerAsync(Customer customer);
		Task<IReadOnlyList<Order?>> GetAllOrdersForCustomer( int id);

	}
}
