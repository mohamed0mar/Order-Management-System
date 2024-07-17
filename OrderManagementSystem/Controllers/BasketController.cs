using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Errors;

namespace E_Commerce.API.Controllers
{

	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		

		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
			
		}


		[HttpGet] //GET : /api/basket  
		public async Task<ActionResult<Basket>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);

			return Ok(basket is null ? new Basket(id) : basket);
		}

		
		[HttpPost] //Post : /api/basket
		public async Task<ActionResult<Basket>> UpdateBasket(Basket basket)
		{
			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
			if (createdOrUpdatedBasket is null)
				return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);
		}

		
		[HttpDelete] //Delete : /api/basket
		public async Task<ActionResult<bool>> DeleteBasket(string id)
		{
			return await _basketRepository.DeleteBasketAsync(id);
		}

	}
}
