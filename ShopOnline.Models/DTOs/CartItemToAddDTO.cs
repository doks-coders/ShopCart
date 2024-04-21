namespace ShopOnline.Models.DTOs
{
	public class CartItemToAddDTO
	{
		public int CartId { get; set; }
		public int ProductId { get; set; }
		public int Qty { get; set; }
	}
}
