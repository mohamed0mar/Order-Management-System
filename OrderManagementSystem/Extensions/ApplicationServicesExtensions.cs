using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Errors;
using OrderManagementSystem.Helper;
using OrderManagementSystem.Repository;
using OrderManagementSystem.Repository.Basket_Repository;
using OrderManagementSystem.Service;
using System.Text;

namespace OrderManagementSystem.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
			services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
			services.AddScoped(typeof(IOrderService), typeof(OrderService));
			services.AddScoped(typeof(IProductService), typeof(ProductService));
			services.AddScoped(typeof(IInvoiceService), typeof(InvoiceService));
			services.AddScoped(typeof(IAuthService), typeof(AuthService));
			services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
			services.AddAutoMapper(typeof(MappingProfiles));


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var erroes = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
													   .SelectMany(P => P.Value.Errors)
													   .Select(E => E.ErrorMessage)
													   .ToList();
					var response = new ApiValidationErrorResponse()
					{
						Errors = erroes
					};
					return new BadRequestObjectResult(response);
				};
			});



			return services;
		}


		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"])),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					};
				});

			return services;

		}

	}
}
