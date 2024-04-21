using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ProductDTO> GetItem(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/Product/{id}");

				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(ProductDTO);
					}
					return await response.Content.ReadFromJsonAsync<ProductDTO>();
				}
				else
				{
					var message = await response.Content.ReadAsStringAsync();
					throw new Exception(message);
				}

			}
			catch (Exception ex)
			{
				string message = ex.Message;
				throw new Exception(message);
			}
		}

		public async Task<IEnumerable<ProductDTO>> GetItems()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/Product");

				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(IEnumerable<ProductDTO>);
					}
					return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();

				}
				else
				{
					string message = await response.Content.ReadAsStringAsync();
					throw new Exception(message);
				}


			}
			catch (Exception ex)
			{
				string message = ex.Message;
				throw;
			}
		}
	}
}
