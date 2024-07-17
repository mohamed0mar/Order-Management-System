using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Core.Entities.Identity;
using OrderManagementSystem.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        
		public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			var authClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.DisplayName),
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.Role,user.Role)
			};

			var userRoles = await userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
				authClaims.Add(new Claim(ClaimTypes.Role, role));

			//AuthKey
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]));
			//Registert Claims

			var token = new JwtSecurityToken(
				audience: _configuration["Jwt:ValidAudience"],
				issuer: _configuration["Jwt:ValidIssuer"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		
	}
}
