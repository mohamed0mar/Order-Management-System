using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(string buyerEmail, int customerId, decimal totalAmount,string  paymentIntentId, PaymentMethod paymentMethod, ICollection<OrderItem> items)
        {
            BuyerEmail = buyerEmail;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            Items = items;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int CustomerId
        {
            get; set;
        }
        public decimal TotalAmount
        {
            get; set;
        }
        public PaymentMethod PaymentMethod
        {
            get; set;
        }

        //Navigatinal property

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();


        public string PaymentIntentId { get; set; }
    }
}
