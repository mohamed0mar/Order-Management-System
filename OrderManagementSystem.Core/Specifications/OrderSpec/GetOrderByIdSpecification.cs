using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Specifications.OrderSpec
{
    public class GetOrderSpecifications:BaseSpecifications<Order>
	{
		public GetOrderSpecifications(int orderId)
		  : base(O => O.Id == orderId)
		{
			Includes.Add(O => O.Items);

		}
		public GetOrderSpecifications()
		  : base()
		{
			Includes.Add(O => O.Items);

		}
	}
}
