using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts
{
	public interface IShoppingCartService
	{
		Task<List<CartItemDTO>> GetItems(int userId);
		Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAdd);
		Task<CartItemDTO> DeleteItem(int cartItemId);
		Task<CartItemDTO> UpdateQty(CartItemQtyUpdatedDTO cartItemQtyUpdatedDTO);
		event Action<int> OnShoppingCartChanged;
		void RaiseEventOnShopping(int totalQty);
	}
}
