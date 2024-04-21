using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Helpers;
using ShopOnline.Web.Services.Contracts;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ShopOnline.Web.Pages
{
	public class ShoppingCartBase : ComponentBase
	{
		public ShoppingCartBase()
		{
			ShoppingCartItems = new ();
			ShoppingCartItems.CollectionChanged += CartChanged;
		}
		[Inject]
		public IShoppingCartService ShoppingCartService { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Inject]
		public IJSRuntime Js { get; set; }

		public ObservableCollection<ObservableItem<CartItemDTO>> ShoppingCartItems { get; set; }

		

		public decimal TotalPrice { get; set; }
		public decimal TotalQuantity { get; set; }
		public string ExceptionMessage { get; set; }

		
		protected override async Task OnInitializedAsync()
		{
			try
			{
				var shoppingCartListItems = await ShoppingCartService.GetItems(HardCoded.UserId);

				shoppingCartListItems.ForEach(val =>
				{
					var item = new ObservableItem<CartItemDTO>(val);
					item.PropertyChanged += CartChanged;
					ShoppingCartItems.Add(item);
				});

				
			}
			catch (Exception ex)
			{
				ExceptionMessage = ex.Message;
			}

		}



		protected async Task UpdateQtyCartItem_Click(int id,  int qty)
		{
			try
			{
				if (qty > 0)
				{
					var updateItemDTO = new CartItemQtyUpdatedDTO()
					{
						CartItemId = id,
						Qty = qty
					};
					var returnUpdateItemDTO = await ShoppingCartService.UpdateQty(updateItemDTO);
					UpdateItem(returnUpdateItemDTO);
					await MakeUpdateQtyButtonVisible(id, false);
				}
				else
				{
					var item = GetCartItem(id);
					if(item != null)
					{
						item.Value.Qty = 1;
						item.Value.TotalPrice = item.Value.Price;
					}
				}
			}catch(Exception ex)
			{
				ExceptionMessage = ex.Message;
			}
		}

		protected async Task DeleteFromCart_Click(int id)
		{
			try
			{
				var deletedCartItem = await ShoppingCartService.DeleteItem(id);

				RemoveCartItem(deletedCartItem.Id);
				
			}
			catch (Exception ex)
			{
				ExceptionMessage = ex.Message;
			}

		}

		protected async Task UpdateQty_Input(int id)
		{
			await MakeUpdateQtyButtonVisible(id, true);
		}

		private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
		{
			await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
		}



		private void RemoveCartItem(int id)
		{
			var cartItem = GetCartItem(id);

			ShoppingCartItems.Remove(cartItem);
		}

		private ObservableItem<CartItemDTO> GetCartItem(int Id)
		{
			return ShoppingCartItems.FirstOrDefault(u => u.Value.Id == Id);
		}

		private void UpdateItem(CartItemDTO returnUpdateItemDTO)
		{
			ObservableItem<CartItemDTO> updatedCartItem = GetCartItem(returnUpdateItemDTO.Id);
			if(updatedCartItem != null)
			{
				var item = ShoppingCartItems.Where(u => u.Value.Id == updatedCartItem.Value.Id).FirstOrDefault();
				item.Value = returnUpdateItemDTO;
			}
		}

		private void CartChanged(object? sender = null, NotifyCollectionChangedEventArgs? e = null)
		{
			CalculateSummaryTotals();
		}
			
		private void CartChanged(object? sender=null,  PropertyChangedEventArgs? e=null)
		{
			CalculateSummaryTotals();
		}

		private void CalculateSummaryTotals()
		{
			SetTotalPrice();
			SetTotalQty();
			ShoppingCartService.RaiseEventOnShopping((int)TotalQuantity);
		}







		private void SetTotalPrice()
		{
			TotalPrice = ShoppingCartItems.Sum(u => u.Value.TotalPrice); ;
		}
		private void SetTotalQty() 
		{
			TotalQuantity = ShoppingCartItems.Sum(u => u.Value.Qty);
		}

	}
}
