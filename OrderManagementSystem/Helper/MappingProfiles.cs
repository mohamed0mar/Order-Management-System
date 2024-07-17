using AutoMapper;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Order;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.DTOs.Order;

namespace OrderManagementSystem.Helper
{
    public class MappingProfiles:Profile
	{
        public MappingProfiles()
        {
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<Order, OrderToReturnDto>();
			CreateMap<OrderItem, OrderItemDto>();
			CreateMap<Customer, CustomerDto>();

		}
    }
}
