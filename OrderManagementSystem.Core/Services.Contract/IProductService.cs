using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services.Contract
{
	public interface IProductService
	{
		Task<Product?> AddProductAsync(Product Product);
		Task<Product?> UpdateProductAsync(int productId,Product product);
	}
}
