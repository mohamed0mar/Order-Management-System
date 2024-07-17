
using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Repositories.Contract;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderManagementSystem.Repository.Basket_Repository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
		public BasketRepository(IConnectionMultiplexer Redis)
		{
			_database = Redis.GetDatabase();
		}


		public async Task<Basket?> GetBasketAsync(string id)
		{
			var basket = await _database.StringGetAsync(id);

			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(basket);
		}

		public async Task<Basket?> UpdateBasketAsync(Basket basket)
		{
			var createdOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
			if (!createdOrUpdatedBasket)
				return null;
			return await GetBasketAsync(basket.Id);
		}

		public async Task<bool> DeleteBasketAsync(string id)
		{
			return await _database.KeyDeleteAsync(id);
		}
	}
}
