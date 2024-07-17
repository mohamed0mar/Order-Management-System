using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Core.Specifications.OrderSpec;
using Stripe;
using Invoice = OrderManagementSystem.Core.Entities.Order.Invoice;
using Product = OrderManagementSystem.Core.Entities.Product;

namespace OrderManagementSystem.Service
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderService(
			IBasketRepository basketRepository,
			IUnitOfWork unitOfWork,
			IPaymentService paymentService
			)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}
		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int customerId)
		{

			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepository.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo

			var orderItems = new List<OrderItem>();
			var productRepo = _unitOfWork.Repository<Product>();
			decimal totalPrice = 0;
			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					var unitPrice = product.Price;
					var quantity = item.Quantity;

					if (item.Quantity > product.Stock)
					{
						throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'. Available stock: {product.Stock}");
					}

					//if (item.Quantity > product.Stock)
					//{
					//	return new BadRequestObjectResult(new { message = $"Insufficient stock for product '{product.Name}'. Available stock: {product.Stock}" });
					//}


					totalPrice += unitPrice * quantity;

					var discount = CalculateDiscount(totalPrice);

					var orderItem = new OrderItem
					{
						ProductId = product.Id,
						Quantity = quantity,
						UnitPrice = unitPrice,
						Discount = discount
					};

					orderItems.Add(orderItem);
				}
			}


			//Check if there are Order or not
			var orderRepo = _unitOfWork.Repository<Order>();
			var spec = new OrderWithPaymentIntentSpecifications(basket?.PaymentIntentId);

			var existingOrder = await orderRepo.GetWithSpecAsync(spec);
			if (existingOrder is not null)
			{
				orderRepo.Delete(existingOrder);
				await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			}

			// 3. Create Order Object
			var order = new Order
			(
				buyerEmail: buyerEmail,
				customerId: customerId,
				paymentMethod: Core.Entities.Order.PaymentMethod.CreditCard,
				totalAmount: totalPrice - orderItems.Sum(item => item.Discount),
				items: orderItems,
				paymentIntentId: basket?.PaymentIntentId ?? ""
			);

			orderRepo.Add(order);

			var invoice = new Invoice
			{
				OrderId = order.Id,
				 InvoiceDate = DateTime.UtcNow,
				TotalAmount = order.TotalAmount,
				Order=order,		
				
			};

			_unitOfWork.Repository<Invoice>().Add(invoice);



			foreach (var item in basket.Items)
			{
				var product = await productRepo.GetAsync(item.Id);
				product.Stock -= item.Quantity; // Deduct ordered quantity from available stock
				_unitOfWork.Repository<Product>().Update(product);
			}

			int result = await _unitOfWork.CompleteAsync();


			if (result <= 0)
				return null;
			return order;
		}

		public async Task<Order?> GetOrderByIdAsync(int orderId)
		{
			var spec = new GetOrderSpecifications(orderId);
			var order=await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
			return order;
		}

		public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
		{
			var spec=new GetOrderSpecifications();
			var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
			return orders;
		}

		public async Task<Order?> UpdateOrderStatusAsync(int orderId, string newStatus)
		{
			var order = await _unitOfWork.Repository<Order>().GetAsync(orderId);
			if (order is null)
				return null;

			if (!Enum.TryParse(typeof(OrderStatus), newStatus, out var status))
			{
				return null; 
			}

			order.Status = (OrderStatus)status;
			_unitOfWork.Repository<Order>().Update(order);
			var result = await _unitOfWork.CompleteAsync() > 0;

			if (!result)
				return null; 

			return order;
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
