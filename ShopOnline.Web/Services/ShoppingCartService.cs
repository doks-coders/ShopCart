using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ShopOnline.Web.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly HttpClient _httpClient;

		public event Action<int> OnShoppingCartChanged;

		public ShoppingCartService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<CartItemDTO> AddItem(CartItemToAddDTO cartItemToAdd)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/shoppingcart", cartItemToAdd);
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(CartItemDTO);
					}
					return await response.Content.ReadFromJsonAsync<CartItemDTO>();
				}
				else
				{
					var message = await response.Content.ReadAsStringAsync();
					throw new Exception($"Http Error Code: {response.StatusCode}, Message:{message}");
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<List<CartItemDTO>> GetItems(int userId)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return new List<CartItemDTO>();	
					}
					return await response.Content.ReadFromJsonAsync<List<CartItemDTO>>();
				}
				else
				{
					var message = response.Content.ReadAsStringAsync();
					throw new Exception($"Http Error Code: {response.StatusCode}, Message:{message}");
				}

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<CartItemDTO> DeleteItem(int cartItemId)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{cartItemId}");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(CartItemDTO);
					}
					return await response.Content.ReadFromJsonAsync<CartItemDTO>();
				}
				else
				{
					var message = response.Content.ReadAsStringAsync();
					throw new Exception($"Http Error Code: {response.StatusCode}, Message:{message}");
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			
		}

		public async Task<CartItemDTO> UpdateQty(CartItemQtyUpdatedDTO cartItemQtyUpdatedDTO)
		{
			try
			{
				var jsonRequest = JsonSerializer.Serialize(cartItemQtyUpdatedDTO);
				var content = new StringContent(jsonRequest, Encoding.UTF8,"application/json-patch+json");
				var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdatedDTO.CartItemId}", content);
				if (response.IsSuccessStatusCode)
				{

					return await response.Content.ReadFromJsonAsync<CartItemDTO>();
				}
				return null;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void RaiseEventOnShopping(int totalQty)
		{
			if (OnShoppingCartChanged != null)
			{
				OnShoppingCartChanged?.Invoke(totalQty);
			}
			
		}

	

	}
}
