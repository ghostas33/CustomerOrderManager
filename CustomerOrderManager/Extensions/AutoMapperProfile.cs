using AutoMapper;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Models.Entities;

namespace CustomerOrderManager.Extensions;

/// <summary>
/// AutoMapper profile for mapping between entities, DTOs, and commands
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();
        CreateMap<CreateCustomerDto, CreateCustomerCommand>();
        CreateMap<UpdateCustomerDto, UpdateCustomerCommand>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        CreateMap<UpdateOrderCommand, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        CreateMap<CreateOrderDto, CreateOrderCommand>();
        CreateMap<UpdateOrderDto, UpdateOrderCommand>();

        // OrderItem mappings
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.UnitPrice, opt => opt.Ignore()); // Will be set from Product

        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<CreateProductDto, CreateProductCommand>();
        CreateMap<UpdateProductDto, UpdateProductCommand>();
    }
}