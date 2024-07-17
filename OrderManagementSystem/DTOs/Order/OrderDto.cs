using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.DTOs.Order
{
	public class OrderDto
	{
		[Required]
		public string BasketId { get; set; } = null!;
        public int CustomerId { get; set; }
    }
}
