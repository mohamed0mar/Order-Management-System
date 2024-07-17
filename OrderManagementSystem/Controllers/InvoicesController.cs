using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Errors;

namespace OrderManagementSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class InvoicesController : BaseApiController
	{

		//All THis Controller is just for Admin 

		private readonly IInvoiceService _invoiceService;

		public InvoicesController(IInvoiceService invoiceService)
        {
			_invoiceService = invoiceService;
		}

		[HttpGet("{invoiceId}")] //GET : api/invoices/1
		public async Task<ActionResult<Invoice?>> GetInvoiceById(int invoiceId)
		{
			var invoice=await _invoiceService.GetInvoiceByIdAsync(invoiceId);
			if (invoice is null)
				return NotFound(new ApiResponse(404));
			return Ok(invoice);
		}



		[HttpGet] //GET : api/invoices
		public async Task<ActionResult<IReadOnlyList<Invoice>>> GetAllInvoices()
		{
			var invoices = await _invoiceService.GetAllInvoicesAsync();
			
			return Ok(invoices);
		}

	}
}
