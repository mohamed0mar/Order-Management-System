namespace OrderManagementSystem.Core.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }

        public OrderItem(int orderId, int productId, int quantity, decimal unitPrice, decimal discount)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
        }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

    }
}