using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Errors;

namespace OrderManagementSystem.Controllers
{

	public class CustomersController : BaseApiController
	{
		private readonly ICustomerService _customerService;
		private readonly IMapper _mapper;

		public CustomersController(
			ICustomerService customerService,
			IMapper mapper
			)
        {
			_customerService = customerService;
			_mapper = mapper;
		}

		[HttpPost] //POST /api/customers
		[ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] Customer customer)
		{
			var createdcustomer =await _customerService.CreateCustomerAsync(customer);
			if (createdcustomer is null)
				return BadRequest(new ApiResponse(400));

			return Ok(_mapper.Map<CustomerDto>(createdcustomer));
		}

		[HttpGet("{customerId}")] //GET /api/customers/1
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IReadOnlySet<OrderToReturnDto?>>> GetOrders(int customerId)
		{
			var orders=await _customerService.GetAllOrdersForCustomer(customerId);
			if (orders is null || orders.Count==0)
				return NotFound(new ApiResponse(404));

			return  Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
		}
	}
}
