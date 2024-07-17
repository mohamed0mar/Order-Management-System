using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.Core.Services.Contract;
using OrderManagementSystem.Core.Specifications.InvoiceSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Service
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IUnitOfWork _unitOfWork;

		public InvoiceService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

        public async Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync()
		{
			var spec = new GetInvoiceSpecifications();
			var invoice = await _unitOfWork.Repository<Invoice>().GetAllWithSpecAsync(spec);
			return invoice;
		}
		

		public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
		{
			var spec = new GetInvoiceSpecifications(invoiceId);
			var invoice = await _unitOfWork.Repository<Invoice>().GetWithSpecAsync(spec);
			return invoice;
		}
		
	}
}
