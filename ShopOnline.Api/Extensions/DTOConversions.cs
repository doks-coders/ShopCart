using ShopOnline.Api.Entities;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Api.Extensions
{
	public static class DTOConversions
	{
		public static IEnumerable<ProductDTO> ConvertToDTO(this IEnumerable<Product> products,
			IEnumerable<ProductCategory> productCategories)
		{
			return (from product in products
					join productCategory in productCategories
					on product.CategoryId equals productCategory.Id
					select new ProductDTO
					{
						Id = product.Id,
						Name = product.Name,
						Description = product.Description,
						ImageURL = product.ImageURL,
						Price = product.Price,
						Qty = product.Qty,
						CategoryId = product.CategoryId,
						CategoryName = productCategory.Name
					}).ToList();
		}

		public static ProductDTO ConvertToDTO(this Product product,
			ProductCategory productCategory)
		{
			return new ProductDTO
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				ImageURL = product.ImageURL,
				Price = product.Price,
				Qty = product.Qty,
				CategoryId = product.CategoryId,
				CategoryName = productCategory.Name
			};
		}

		public static IEnumerable<CartItemDTO> ConvertToDTO(this IEnumerable<CartItem> cartItems,
			IEnumerable<Product> products)
		{
			var items =  (from cartItem in cartItems
							  join product in products
							  on cartItem.ProductId equals product.Id
							  select new CartItemDTO()
							  {
								  Id = cartItem.Id,
								  CartId = cartItem.CartId,
								  ProductId = cartItem.ProductId,
								  ProductName = product.Name,
								  ProductDescription = product.Description,
								  ProductImageURL = product.ImageURL,
								  Price = product.Price,
								  TotalPrice = product.Price * cartItem.Qty,
								  Qty = cartItem.Qty
							  }).ToList();
			return items;
		}

		public static CartItemDTO ConvertToDTO(this CartItem cartItem,
			Product product)
		{
			return new CartItemDTO()
			{
				Id = cartItem.Id,
				CartId = cartItem.CartId,
				ProductId = cartItem.ProductId,
				ProductName = product.Name,
				ProductDescription = product.Description,
				ProductImageURL = product.ImageURL,
				Price = product.Price,
				TotalPrice = product.Price * cartItem.Qty,
				Qty = cartItem.Qty
			};
		}

	}
}
