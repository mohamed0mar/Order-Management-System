using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Repositories.Contract;
using OrderManagementSystem.Core.Specifications;
using OrderManagementSystem.Repository._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository.Generic_Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly OrderManagementSystemDbContext _dbContext;

		public GenericRepository(OrderManagementSystemDbContext dbContext)
        {
			_dbContext = dbContext;
		}
        public async Task<IReadOnlyList<T>> GetAllAsync()
			=>await _dbContext.Set<T>().ToListAsync();

		public async Task<T?> GetAsync(int id)
			=> await _dbContext.Set<T>().FindAsync(id);
		

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		}

		public IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
		{
			return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}
		public void Add(T entity)
			=> _dbContext.Add(entity);

		public void Delete(T entity)
			=> _dbContext.Remove(entity);

		public void Update(T entity)
			=> _dbContext.Update(entity);
	}
}
