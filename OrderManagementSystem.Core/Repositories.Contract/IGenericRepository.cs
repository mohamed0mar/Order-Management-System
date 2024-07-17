using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Specifications;

namespace OrderManagementSystem.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T?> GetAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
