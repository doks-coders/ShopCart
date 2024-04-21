using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
	public class ProductBase : LayoutComponentBase
	{
		[Inject]
		public IProductService ProductService { get; set; }


		[Inject]
		public IShoppingCartService ShoppingCartService { get; set; }

		public IEnumerable<ProductDTO> Products { get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				Products = await ProductService.GetItems();
				var shoppingCart = await ShoppingCartService.GetItems(HardCoded.UserId);
				var totalQty = shoppingCart.Sum(u => u.Qty);
				ShoppingCartService.RaiseEventOnShopping(totalQty);
			}
			catch (Exception ex)
			{
			}

		}
		protected IOrderedEnumerable<IGrouping<int, ProductDTO>> GetGroupProductsByCategory()
		{
			return (from product in Products
					group product by product.CategoryId into productCategoryGroup
					orderby productCategoryGroup.Key
					select productCategoryGroup);
		}
		protected string GetCategoryName(IGrouping<int, ProductDTO> groupedCategory)
		{
			return groupedCategory.FirstOrDefault(u => u.CategoryId == groupedCategory.Key).CategoryName;
		}



	}
}
