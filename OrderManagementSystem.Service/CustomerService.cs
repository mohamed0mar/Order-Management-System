using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Core.Specifications.CustomerSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
    public class CustomerService: ICustomerService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CustomerService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

		public async Task<Customer?> CreateCustomerAsync(Customer customer)
		{
			_unitOfWork.Repository<Customer>().Add(customer);
			int result=await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;
			return customer;
		}

		public Task<IReadOnlyList<Order?>> GetAllOrdersForCustomer( int id)
		{
			var orderRepo = _unitOfWork.Repository<Order>();

			var spec = new GetAllOrdersForCustomerSpecification( id);
			var orders=orderRepo.GetAllWithSpecAsync(spec);
			if (orders is null)
				return null;
			return orders;
		}

	
	}
}
