using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core
{
	public interface IUnitOfWork:IAsyncDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

		Task<int> CompleteAsync();
	}
}
