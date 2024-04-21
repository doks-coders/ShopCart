using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductCategoryBase:ComponentBase
    {
		[Inject]
        public IManageProductLocalStorageService ManageProductLocalStorageService {  get; set; }
		public List<ProductDTO> Products { get; set; } = new();


		protected override async Task OnInitializedAsync()
		{
			var products = await ManageProductLocalStorageService.GetCollection();
			Products = products.ToList();
			
			
		}

	
	}
}
