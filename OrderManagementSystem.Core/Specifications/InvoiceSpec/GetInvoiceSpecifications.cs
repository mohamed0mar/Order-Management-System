using OrderManagementSystem.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Specifications.InvoiceSpec
{
	public class GetInvoiceSpecifications:BaseSpecifications<Invoice>
	{
		public GetInvoiceSpecifications(int invoiceId)
			: base(I => I.Id==invoiceId)
		{
			Includes.Add(I => I.Order);
			Includes.Add(I => I.Order.Items);
		}

		public GetInvoiceSpecifications()
		: base()
		{
			Includes.Add(I => I.Order);
			Includes.Add(I => I.Order.Items);
		}
	}
}
