using Microsoft.Extensions.Configuration;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Core.Specifications.OrderSpec;
using Stripe;
using Product = OrderManagementSystem.Core.Entities.Product;

namespace OrderManagementSystem.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(
			IConfiguration configuration,
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork)
        {
			_configuration = configuration;
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}
        public async Task<Basket?> CreateOrUpdatePaymentIntent(string basketId)
		{
			//1. Get Secret Key
			StripeConfiguration.ApiKey = _configuration["StripeSetting:SecretKey"];

			//2. Get Basket from Basket Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);
			if (basket is null)
				return null;

			//3. Check If The Price Is Correct Or Not
			if (basket?.Items?.Count > 0)
			{
				var productRepo = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;
				}
			}

			//4. Create Or Update Payment Intent 

			PaymentIntent paymentIntent;
			PaymentIntentService paymentIntentService = new PaymentIntentService();

			if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create Payment Intent
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long?)basket.Items.Sum(item => item.Price * 100 * item.Quantity),
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card" }
				};
				paymentIntent = await paymentIntentService.CreateAsync(options); //Integration with stripe
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else  //Update Payment Intent
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long?)basket.Items.Sum(item => item.Price * 100 * item.Quantity )
				};
				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
			}

			await _basketRepo.UpdateBasketAsync(basket);
			return (basket);
		}

		public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
		{
			var orderRepo = _unitOfWork.Repository<Order>();

			var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);

			var order = await orderRepo.GetWithSpecAsync(spec);

			if (order is null)
				return null;

			if (isPaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			orderRepo.Update(order);

			await _unitOfWork.CompleteAsync();

			return (order);
		}
	}
}
