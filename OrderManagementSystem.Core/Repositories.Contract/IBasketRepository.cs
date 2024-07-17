using OrderManagementSystem.Core.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Repositories.Contract
{
	public interface IBasketRepository
	{
		Task<Basket?> GetBasketAsync(string id);

		Task<Basket?> UpdateBasketAsync(Basket basket);

		Task<bool> DeleteBasketAsync(string id);
	}
}
