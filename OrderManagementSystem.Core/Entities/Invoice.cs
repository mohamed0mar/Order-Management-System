using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities.Order
{
    public class Invoice:BaseEntity
	{
		
		public int OrderId { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

		public Order Order { get; set; } = null!;

		
	}
}
