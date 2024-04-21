using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts
{
	public interface IManageProductLocalStorageService
	{
		Task<IEnumerable<ProductDTO>> GetCollection();
		Task RemoveCollection();
	}
}
