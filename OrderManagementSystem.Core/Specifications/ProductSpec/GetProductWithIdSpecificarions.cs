using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Specifications.ProductSpec
{
	public class GetProductSpecificarions:BaseSpecifications<Product>
	{
        public GetProductSpecificarions(int id)
            :base(P=>P.Id==id)
        {
			Includes.Add(P => P.OrderItems);
        }

		public GetProductSpecificarions()
		   : base()
		{
			Includes.Add(P => P.OrderItems);
			
		}
	}
}
