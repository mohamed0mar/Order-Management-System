using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository._Data
{
    public static class OrderManagementSystemDataSeed
    {
        public static async Task SeedAsync(OrderManagementSystemDbContext _dbContext)
        {
            if (_dbContext.Products.Count() == 0)
            {
                var productData = File.ReadAllText("../OrderManagementSystem.Repository/_Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                        _dbContext.Set<Product>().Add(product);
                    await _dbContext.SaveChangesAsync();
                }
            }


        }

    }
}
