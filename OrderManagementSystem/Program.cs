
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderManagementSystem.Core.Entities.Identity;
using OrderManagementSystem.Extensions;
using OrderManagementSystem.Repository._Data;
using OrderManagementSystem.Repository._Identity;
using StackExchange.Redis;

namespace OrderManagementSystem
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configer Service
			// Add services to the container.

			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<OrderManagementSystemDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddDbContext<ApplicationIdentityContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityContext>();

			builder.Services.AddAuthServices(builder.Configuration);

			builder.Services.AddApplicationServices();

			#endregion



			var app = builder.Build();

			#region Apply Maigrations | DataSeed

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			//Ask CLR to create object from OrderManagementSystemDbContext Exceplicitly
			var _dbContext = services.GetRequiredService<OrderManagementSystemDbContext>();
			var _identityDbContext = services.GetRequiredService<ApplicationIdentityContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync();
				await _identityDbContext.Database.MigrateAsync();

				await OrderManagementSystemDataSeed.SeedAsync(_dbContext);

			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "There Is An Error Occured During You Apply Maigration");
			}

			#endregion


			#region Configer | Middelwere |Pipline

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			// Authentication and Authorization
			app.UseAuthentication();	
			app.UseAuthorization();

			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
