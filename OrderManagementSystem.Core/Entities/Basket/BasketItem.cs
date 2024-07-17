namespace OrderManagementSystem.Core.Entities.Basket
{
	public class BasketItem
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}