using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services.Contract
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
	}
}
