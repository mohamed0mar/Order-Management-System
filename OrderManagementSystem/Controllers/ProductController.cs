using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Core.Specifications.ProductSpec;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Errors;

namespace OrderManagementSystem.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public ProductsController(
			IUnitOfWork unitOfWork, 
			IProductService productService,
			IMapper mapper
			)
		{
			_unitOfWork = unitOfWork;
			_productService = productService;
			_mapper = mapper;
		}

		[HttpGet] //GET : /api/products 
		public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
		{
			var spec = new GetProductSpecificarions();
			var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

			return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
		}


		[HttpGet("{id}")] //GEt : /api/products/1
		[ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var spec = new GetProductSpecificarions(id);
			var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
			if (product is null)
				return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<ProductDto>(product));
		}


		[Authorize(Roles = "Admin")]
		[HttpPost] //POST : /api/products
		public async Task<ActionResult<Product?>> AddProduct(ProductDto productDto)
		{
			var product=_mapper.Map<Product>(productDto);
			var addProduct = await _productService.AddProductAsync(product);
			if (addProduct is null)
				return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<ProductDto>(addProduct));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{productId}")]//PUT : /api/products/2
		public async Task<ActionResult<Product?>> UpdateProduct(int productId, ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);
			var updateProduct=await _productService.UpdateProductAsync(productId, product);
			if (updateProduct is null)
				return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<ProductDto>(updateProduct));
		}
	}
}
