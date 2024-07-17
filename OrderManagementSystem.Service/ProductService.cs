using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<Product?> AddProductAsync(Product Product)
		{
			 _unitOfWork.Repository<Product>().Add(Product);
			var count=await _unitOfWork.CompleteAsync();
			if (count == 0)
				return null;
			return Product;
				
		}

		public async Task<Product?> UpdateProductAsync(int productId, Product product)
		{
			var productRepo= _unitOfWork.Repository<Product>();
			var existingProduct=await productRepo.GetAsync(productId);
			if (existingProduct is null) 
				return null;

			existingProduct.Name = product.Name;
			existingProduct.Price = product.Price;
			existingProduct.Stock = product.Stock;

			productRepo.Update(existingProduct);
			var count = await _unitOfWork.CompleteAsync();
			if(count == 0)
				return null;
			return existingProduct;
		}
	}
}
