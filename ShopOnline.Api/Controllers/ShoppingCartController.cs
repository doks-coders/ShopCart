using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingCartController : ControllerBase
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IProductRepository _productRepository;

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_productRepository = productRepository;
		}

		[HttpGet("{userId:int}/GetItems")]
		public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetItems(int userId)
		{
			try
			{
				var cartItems = await _shoppingCartRepository.GetItems(userId);

				if (cartItems == null) return NotFound();
				var products = await _productRepository.GetItems();
				if (products == null) return NotFound();

				var cartItemDTO = cartItems.ConvertToDTO(products);

				return Ok(cartItemDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		[HttpGet("{cartId:int}")]
		public async Task<ActionResult<CartItemDTO>> GetItem(int cartId)
		{
			try
			{
				var cartItem = await _shoppingCartRepository.GetItem(cartId);

				if (cartItem == null) return NotFound();
				var product = await _productRepository.GetItem(cartItem.ProductId);
				if (product == null) return NotFound();

				var cartItemDTO = cartItem.ConvertToDTO(product);

				return Ok(cartItemDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		[HttpPost]
		public async Task<ActionResult> PostItem([FromBody] CartItemToAddDTO cartItemToAdd)
		{
			try
			{
				var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAdd);
				if (newCartItem == null)
				{
					return NoContent();
				}
				var product = await _productRepository.GetItem(newCartItem.ProductId);
				if (product == null)
				{
					throw new Exception($"Something went wrong when attempting to retrieve product. (product:({cartItemToAdd.ProductId})) ");
				}

				var newCartItemDTO = newCartItem.ConvertToDTO(product);
				/**
				    that it is standard practice for a post

					action method to return the location of

					the resource where the newly added item

					can be found.

				
					this location will be returned in the

					header of the http response returned

					from this method
				 */
				return CreatedAtAction(nameof(GetItem),new{ cartId = newCartItemDTO.Id },newCartItemDTO);


			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteItem(int id)
		{
			try
			{
				var item = await _shoppingCartRepository.DeleteItem(id);
				if(item == null)
				{
					return NotFound();
				}
				var product = await _productRepository.GetItem(item.ProductId);
				if (product == null) return NotFound();
				var caritemDTO =  item.ConvertToDTO(product);

				return Ok(caritemDTO);
			}
			catch(Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message) ;
			}
			
		}


		[HttpPatch("{id:int}")]
		public async Task<ActionResult<CartItemDTO>> UpdateQty(int id, CartItemQtyUpdatedDTO cartItemQtyUpdatedDTO)
		{
			try
			{
				var cartItem = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdatedDTO);
				if(cartItem == null) return NotFound();
				var product = await _productRepository.GetItem(cartItem.ProductId);
				if (product == null) return NotFound();
				var cartItemDTO = cartItem.ConvertToDTO(product);
				return Ok(cartItemDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
