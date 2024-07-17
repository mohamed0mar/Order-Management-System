using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Core.Entities.Order;

namespace OrderManagementSystem.Core.Entities
{
    public class Product:BaseEntity
	{
		public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        //Navigatinal property
        public ICollection<OrderItem> OrderItems { get; set; }=new HashSet<OrderItem>();
    }
}
