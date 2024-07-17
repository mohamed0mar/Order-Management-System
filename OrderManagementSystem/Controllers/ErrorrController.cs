using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Errors;

namespace OrderManagementSystem.Controllers
{
	[Route("errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorrController : ControllerBase
	{
		public ActionResult Error(int code)
		{
			if (code == 400)
				return Unauthorized(new ApiResponse(400));
			else if (code == 404)
				return NotFound(new ApiResponse(404));
			else
				return StatusCode(code);
		}
	}
}
