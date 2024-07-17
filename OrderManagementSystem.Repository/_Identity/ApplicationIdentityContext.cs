using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository._Identity
{
	public class ApplicationIdentityContext: IdentityDbContext<ApplicationUser>
	{
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
            :base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
