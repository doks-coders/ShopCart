using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
	public class ProductDetailsBase:ComponentBase
	{
		[Parameter]
		public int Id { get; set; }

		[Inject]
		public IProductService ProductService { get; set; }

		[Inject]
		public IShoppingCartService ShoppingCartService { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }	

		public ProductDTO Product { get; set; }

		public string ExceptionMessage { get; set; }	

		protected override async Task OnInitializedAsync()
		{
			try
			{
				Product = await ProductService.GetItem(Id);
			}
			catch(Exception ex)
			{
				ExceptionMessage = ex.Message;	
			}
			
		}

		protected async Task AddToCart_Click(CartItemToAddDTO cartItemToAdd)
		{
			try
			{
				var response = await ShoppingCartService.AddItem(cartItemToAdd);
				NavigationManager.NavigateTo("/ShoppingCart");
			}
			catch(Exception ex)
			{
				ExceptionMessage = ex.Message;
			}
		}

	}
}
