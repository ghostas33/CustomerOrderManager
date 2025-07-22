using MediatR;
using AutoMapper;
using CustomerOrderManager.Queries;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for getting all orders
/// </summary>
public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders.FindAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}

/// <summary>
/// Handler for getting an order by ID
/// </summary>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.FindByIdAsync(request.Id);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }
}

/// <summary>
/// Handler for getting orders by customer ID
/// </summary>
public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders.FindByCustomerIdAsync(request.CustomerId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}

/// <summary>
/// Handler for getting orders by date range
/// </summary>
public class GetOrdersByDateRangeQueryHandler : IRequestHandler<GetOrdersByDateRangeQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders.FindByDateRangeAsync(request.StartDate, request.EndDate);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}