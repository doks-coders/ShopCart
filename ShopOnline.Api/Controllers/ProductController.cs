using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository _productRepository;

		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetItems()
		{
			try
			{
				var products = await _productRepository.GetItems();
				var productCategories = await _productRepository.GetCategories();
				if (products == null || productCategories == null)
				{
					return NotFound();
				}
				else
				{
					return Ok(products.ConvertToDTO(productCategories));
				}
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					"Error Retrieving Data From Database");
			}

		}



		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductDTO>> GetItem(int id)
		{
			try
			{
				var product = await _productRepository.GetItem(id);
				var productCategory = await _productRepository.GetCategory(product.CategoryId);
				if (product == null || productCategory == null)
				{
					return NotFound();
				}
				else
				{
					return Ok(product.ConvertToDTO(productCategory));
				}
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					"Error Retrieving Data From Database");
			}

		}
	}
}
