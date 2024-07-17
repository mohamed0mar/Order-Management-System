using OrderManagementSystem.Core.Entities;

namespace OrderManagementSystem.DTOs.Order
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public string Status { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public string PaymentIntentId { get; set; } 

    }
}