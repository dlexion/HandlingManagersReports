using AutoMapper;
using ReportHandler.DAL.Contracts.DTO;

namespace ReportHandler.DAL.AutoMapperSetup
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>()
                .ForMember(x => x.Orders, option => option.Ignore());

            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>()
                .ForMember(x => x.Orders, option => option.Ignore());

            CreateMap<Manager, ManagerDTO>();
            CreateMap<ManagerDTO, Manager>()
                .ForMember(x => x.Orders, option => option.Ignore());

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>()
                .ForMember(x => x.ManagerId, option => option.Ignore())
                .ForMember(x => x.CustomerId, option => option.Ignore());
        }

    }
}