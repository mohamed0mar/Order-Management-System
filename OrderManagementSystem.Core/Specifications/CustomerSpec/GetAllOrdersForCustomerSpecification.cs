using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Specifications.CustomerSpec
{
    public class GetAllOrdersForCustomerSpecification:BaseSpecifications<Order>
	{
        public GetAllOrdersForCustomerSpecification( int customerId)
            :base(O=>O.CustomerId==customerId)
        {
            Includes.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);
        }
    }
}
