using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Errors;
using System.Security.Claims;
using Xunit;

namespace UnitTest
{
	public class OrderServiceTests
	{
		private readonly Mock<IOrderService> _orderServiceMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<IUnitOfWork> _mockUnitOfWork;
		private Mock<IBasketRepository> _mockBasketRepository;
		private Mock<IGenericRepository<Product>> _mockProductRepository;
		private readonly OrdersController _controller;

		public OrderServiceTests()
		{
			_orderServiceMock = new Mock<IOrderService>();
			_mapperMock = new Mock<IMapper>();
			_mockUnitOfWork = new Mock<IUnitOfWork>();
			_mockBasketRepository = new Mock<IBasketRepository>();
			_mockProductRepository = new Mock<IGenericRepository<Product>>();
			_controller = new OrdersController(_orderServiceMock.Object, _mapperMock.Object);

			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Email, "mohamedmahmoud.omar25@gmail.com")
			}, "mock"));

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = user }
			};


		}


		[Fact]
		public async Task CreateOrder_ReturnsBadRequest_WhenOrderIsNull()
		{
			// Arrange
			_orderServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Order)null);

			// Act
			var result = await _controller.CreateOrder(new OrderDto { BasketId = "basket1", CustomerId = 1 });

			// Assert
			var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			Assert.IsType<ApiResponse>(actionResult.Value);
		}


		[Fact]
		public async Task CreateOrder_ReturnsOrder_WhenOrderIsNotNull()
		{
			// Arrange
			var order = new Order();
			_orderServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(order);
			_mapperMock.Setup(m => m.Map<OrderToReturnDto>(It.IsAny<Order>())).Returns(new OrderToReturnDto());

			// Act
			var result = await _controller.CreateOrder(new OrderDto { BasketId = "basket1", CustomerId = 1 });

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.IsType<OrderToReturnDto>(actionResult.Value);
		}

		[Fact]
		public async Task GetOrder_ReturnsNotFound_WhenOrderIsNull()
		{
			// Arrange
			_orderServiceMock.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

			// Act
			var result = await _controller.GetOrder(1);

			// Assert
			var actionResult = Assert.IsType<NotFoundObjectResult>(result.Result);
			Assert.IsType<ApiResponse>(actionResult.Value);
		}

		[Fact]
		public async Task GetOrder_ReturnsOrder_WhenOrderIsNotNull()
		{
			// Arrange
			var order = new Order();
			_orderServiceMock.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(order);
			_mapperMock.Setup(m => m.Map<OrderToReturnDto>(It.IsAny<Order>())).Returns(new OrderToReturnDto());

			// Act
			var result = await _controller.GetOrder(1);

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.IsType<OrderToReturnDto>(actionResult.Value);
		}

		[Fact]
		public async Task GetAllOrders_ReturnsOrders()
		{
			// Arrange
			var orders = new List<Order> { new Order() };
			_orderServiceMock.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(orders);
			_mapperMock.Setup(m => m.Map<IReadOnlyList<OrderToReturnDto>>(It.IsAny<IReadOnlyList<Order>>())).Returns(new List<OrderToReturnDto>());

			// Act
			var result = await _controller.GetAllOrders();

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.IsType<List<OrderToReturnDto>>(actionResult.Value);
		}

		[Fact]
		public async Task UpdateOrderStatus_ReturnsNotFound_WhenOrderIsNull()
		{
			// Arrange
			_orderServiceMock.Setup(x => x.UpdateOrderStatusAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((Order)null);

			// Act
			var result = await _controller.UpdateOrderStatus(1, new UpdateOrderStatusDto { NewStatus = "Completed" });

			// Assert
			var actionResult = Assert.IsType<NotFoundObjectResult>(result.Result);
			Assert.IsType<ApiResponse>(actionResult.Value);
		}

		[Fact]
		public async Task UpdateOrderStatus_ReturnsOrder_WhenOrderIsNotNull()
		{
			// Arrange
			var order = new Order();
			_orderServiceMock.Setup(x => x.UpdateOrderStatusAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(order);
			_mapperMock.Setup(m => m.Map<OrderToReturnDto>(It.IsAny<Order>())).Returns(new OrderToReturnDto());

			// Act
			var result = await _controller.UpdateOrderStatus(1, new UpdateOrderStatusDto { NewStatus = "Completed" });

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			Assert.IsType<OrderToReturnDto>(actionResult.Value);
		}



		//Update Stock Test

		[Fact]
		public async Task CreateOrder_DeductsStockFromProducts()
		{
			// Arrange
			var basketId = "basket1";
			var customerId = 1;

			var basket = new Basket
			{
				Items = new List<BasketItem>
			{
				new BasketItem { Id = 1, Quantity = 2 },
				new BasketItem { Id = 2, Quantity = 1 }
			}
			};

			_mockBasketRepository.Setup(r => r.GetBasketAsync(basketId)).ReturnsAsync(basket);

			var products = new List<Product>
			{
				new Product { Id = 1, Price = 50, Stock = 10 },
				new Product { Id = 2, Price = 30, Stock = 5 }
			};

			// Mock behavior of product repository to return products with specific stock
			_mockProductRepository.Setup(r => r.GetAsync(It.IsAny<int>()))
								  .ReturnsAsync((int id) => products.FirstOrDefault(p => p.Id == id));

			// Mock repository behavior for updating products
			_mockProductRepository.Setup(r => r.Update(It.IsAny<Product>()))
								  .Callback<Product>(product =>
								  {
									  var originalProduct = products.FirstOrDefault(p => p.Id == product.Id);
									  if (originalProduct != null)
									  {
										  originalProduct.Stock -= product.Stock; // Deduct ordered quantity
									  }
								  });

			var unitOfWork = new Mock<IUnitOfWork>();
			_mockUnitOfWork.SetupGet(uow => uow.Repository<Product>()).Returns(_mockProductRepository.Object);
			_mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

			// Act
			var createdOrder = await _controller.CreateOrder(new OrderDto { BasketId = basketId, CustomerId = customerId });

			// Assert
			Assert.NotNull(createdOrder);

			// Verify that stock was deducted correctly
			foreach (var item in basket.Items)
			{
				var product = products.FirstOrDefault(p => p.Id == item.Id);
				Assert.NotNull(product); // Ensure product exists in mock data

				// Calculate expected stock after deduction
				var expectedStock = product.Stock - item.Quantity;

				// Assert that product stock was updated correctly
				Assert.Equal(expectedStock, product.Stock);
			}
		}

		//Discount Calculation Test
		[Fact]
		public async Task CreateOrder_ReturnsCorrectOrderWithPlaceholderDiscount()
		{
			// Arrange
			var buyerEmail = "mohamedmahmoud.omar25@gmail.com";
			var basketId = "basket1";
			var customerId = 1;

			var basket = new Basket
			{
				Items = new List<BasketItem>
				{
					new BasketItem { Id = 1, Quantity = 2 },
					new BasketItem { Id = 2, Quantity = 1 }
				}
			};

			// Mock behavior of _orderServiceMock.CreateOrderAsync to return an order with placeholder discount
			var mockOrder = new Order
			{
				
				Items = basket.Items.Select(item => new OrderItem
				{
					ProductId = item.Id,
					Quantity = item.Quantity,
					UnitPrice = 50, // Example unit price
					Discount = 0 
				}).ToList(),
					
				TotalAmount = 0	
			};
			_orderServiceMock.Setup(x => x.CreateOrderAsync(buyerEmail, basketId, It.IsAny<int>()))
							 .ReturnsAsync(mockOrder);

			// Act
			var result = await _controller.CreateOrder(new OrderDto { BasketId = basketId, CustomerId = customerId });

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			var orderDto = Assert.IsType<OrderToReturnDto>(actionResult.Value);

			// Example: Assert that the returned OrderToReturnDto has the correct placeholder discount
			foreach (var orderItemDto in orderDto.Items)
			{
				Assert.Equal(0, orderItemDto.Discount);
			}
		}
		public decimal CalculateDiscount(decimal totalPrice)
		{
			decimal discount = 0;


			if (totalPrice > 200)
			{
				discount = totalPrice * 0.10m; // 10% discount for orders over $200
			}
			else if (totalPrice > 100)
			{
				discount = totalPrice * 0.05m; // 5% discount for orders over $100
			}

			// Ensure discount does not exceed total price
			if (discount >= totalPrice)
			{
				discount = totalPrice - 0.01m;
			}

			return discount;
		}
	}
}
