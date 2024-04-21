using ShopOnline.Api.Entities;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Repositories.Contracts
{
	public interface IShoppingCartRepository
	{
		Task<CartItem> AddItem(CartItemToAddDTO cartItemToAddDTO);
		Task<CartItem> UpdateQty(int Id, CartItemQtyUpdatedDTO cartItemQtyUpdatedDTO);
		Task<CartItem> DeleteItem(int Id);
		Task<CartItem> GetItem(int Id);
		Task<IEnumerable<CartItem>> GetItems(int userId);

	}
}
