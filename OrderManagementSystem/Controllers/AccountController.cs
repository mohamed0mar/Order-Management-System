using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities.Identity;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.DTOs.Acount;
using OrderManagementSystem.Errors;

namespace OrderManagementSystem.Controllers
{

    public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthService _authService;

		public AccountController(
            UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IAuthService authService

			)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
		}

		[HttpPost("register")] //POST : /api/Account/register 
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Role=model.Role,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.Phone
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Role= user.Role,
				Email = user.Email,
				Token = "Token"
			});

		}


		[HttpPost("login")] //POST : /api/Account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				return Unauthorized(new ApiResponse(401, "Invalid Login"));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded)
				return Unauthorized(new ApiResponse(401, "Invalid Login"));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Role= user.Role,
				Token = await _authService.CreateTokenAsync(user, _userManager)
			});
		}

	}
}
