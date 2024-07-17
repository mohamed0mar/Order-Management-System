namespace OrderManagementSystem.Core.Entities.Order
{
    public class Customer:BaseEntity
	{
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;

		//Navigatinal property
		public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
