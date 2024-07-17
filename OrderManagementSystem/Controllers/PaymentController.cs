using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities.Basket;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Errors;
using Stripe;

namespace OrderManagementSystem.Controllers
{
	
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		private const string whSecret = "whsec_ee3b96e77697b5ab4c0474e1e9bc8405fd1e51f78b784d204ecff3f51bc0172b";

		public PaymentController(IPaymentService paymentService)
        {
			_paymentService = paymentService;
		}


		[ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("{basketId}")] //POST : /api/payment/{basketId}
		public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null)
				return BadRequest(new ApiResponse(400, "An Error With Your Basket"));
			return Ok(basket);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], whSecret);

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

			// Handle the event
			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
					break;
				case Events.PaymentIntentPaymentFailed:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
					break;
			}

			return Ok();

		}

	}
}
