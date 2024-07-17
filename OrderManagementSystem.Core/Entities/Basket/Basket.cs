using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities.Basket
{
	public class Basket
	{
		public string Id { get; set; } = null!;
        public List<BasketItem> Items { get; set; } = null!;
		public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public Basket()
        {
            
        }
        public Basket(string id)
		{
			Id = id;
			Items = new List<BasketItem>();
		}
	}
}
