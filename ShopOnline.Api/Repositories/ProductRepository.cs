﻿using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ShopOnlineDbContext _dbContext;
		public ProductRepository(ShopOnlineDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<ProductCategory>> GetCategories()
		{
			var categories = await _dbContext.ProductCategories.ToListAsync();
			return categories;
		}

		public async Task<ProductCategory> GetCategory(int id)
		{
			var category = await _dbContext.ProductCategories.FirstOrDefaultAsync(p => p.Id == id);
			return category;
		}

		public async Task<Product> GetItem(int id)
		{
			var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
			return product;
		}

		public async Task<IEnumerable<Product>> GetItems()
		{
			var products = await _dbContext.Products.ToListAsync();
			return products;
		}
	}
}
