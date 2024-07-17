using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Errors;
using System.Security.Claims;

namespace OrderManagementSystem.Controllers
{
	[Authorize]
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(
			IOrderService orderService,
			IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}

		[HttpPost] //POST /api/orders
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody] OrderDto orderDto)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.CreateOrderAsync(email, orderDto.BasketId, orderDto.CustomerId);
			if (order is null)
				return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}



		[HttpGet("{orderId}")] //GET /api/orders/1
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<OrderToReturnDto?>> GetOrder(int orderId)
		{
			var order = await _orderService.GetOrderByIdAsync(orderId);
			if (order is null)
				return NotFound(new ApiResponse(400));
			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}



		[Authorize(Roles = "Admin")]
		[HttpGet] //GET /api/orders
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
		{
			var orders = await _orderService.GetAllOrdersAsync();
			return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
		}


		[Authorize(Roles = "Admin")]
		[HttpPut("{orderId}/status")] //PUT /api/orders/orderId
		public async Task<ActionResult<OrderToReturnDto>> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
		{
			var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, updateOrderStatusDto.NewStatus);
			if (updatedOrder is null)
			{
				return NotFound(new ApiResponse(404, "Order not found or status update failed."));
			}

			return Ok(_mapper.Map<OrderToReturnDto>(updatedOrder));
		}

	}		


}





