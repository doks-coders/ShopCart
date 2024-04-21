using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Repositories
{
	public class ShoppingCartRepository : IShoppingCartRepository
	{
		private readonly ShopOnlineDbContext _shopOnlineDbContext;

		public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
		{
			_shopOnlineDbContext = shopOnlineDbContext;
		}

		private async Task<bool> CartItemExists(int cartId, int productId)
		{
			return await _shopOnlineDbContext.CartItems.AnyAsync(u => u.CartId == cartId && u.ProductId == productId);
		}

		public async Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO)
		{
			if (await CartItemExists(cartItemToAddDTO.CartId, cartItemToAddDTO.ProductId)) return null;

			var item = await (from product in _shopOnlineDbContext.Products
							  where product.Id == cartItemToAddDTO.ProductId
							  select new CartItem()
							  {
								  CartId = cartItemToAddDTO.CartId,
								  ProductId = product.Id,
								  Qty = cartItemToAddDTO.Qty
							  }

							  ).SingleOrDefaultAsync();

			if (item != null)
			{
				var response = await _shopOnlineDbContext.CartItems.AddAsync(item);
				await _shopOnlineDbContext.SaveChangesAsync();
				return response.Entity;
			}
			return null;
		}


		public async Task<CartItem> DeleteItem(int Id)
		{
			var item = await _shopOnlineDbContext.CartItems.FindAsync(Id);
			if(item != null)
			{
				_shopOnlineDbContext.CartItems.Remove(item);
				await _shopOnlineDbContext.SaveChangesAsync();
			}

			return item;

		}

		public async Task<CartItem> GetItem(int Id)
		{
			var item = await (from cart in _shopOnlineDbContext.Carts
						join cartitems in _shopOnlineDbContext.CartItems
						on cart.Id equals cartitems.CartId
						where cartitems.Id == Id
						select new CartItem()
						{
							Id = cartitems.Id,
							CartId = cart.Id,
							ProductId = cartitems.ProductId,
							Qty = cartitems.Qty
						}).FirstOrDefaultAsync();
							  
			return item;
		}

		public async Task<IEnumerable<CartItem>> GetItems(int userId)
		{
			var items = await (from cart in _shopOnlineDbContext.Carts
							  join cartitems in _shopOnlineDbContext.CartItems
							  on cart.Id equals cartitems.CartId
							  where cart.UserId == userId
							  select new CartItem()
							  {
								  CartId = cart.Id,
								  ProductId = cartitems.ProductId,
								  Id = cartitems.Id,
								  Qty = cartitems.Qty
							  })
							  .ToListAsync();
			return items;
		}

		public async Task<CartItem> UpdateQty(int Id, CartItemQtyUpdatedDTO cartItemQtyUpdatedDTO)
		{
			var item = await _shopOnlineDbContext.CartItems.FindAsync(Id);
			if (item != null)
			{
				item.Qty = cartItemQtyUpdatedDTO.Qty;
				await _shopOnlineDbContext.SaveChangesAsync();
				return item;
			}
			return null;
		}
	}
}
