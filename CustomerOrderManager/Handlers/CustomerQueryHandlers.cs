using MediatR;
using AutoMapper;
using CustomerOrderManager.Queries;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for getting all customers
/// </summary>
public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customers.FindAllAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}

/// <summary>
/// Handler for getting a customer by ID
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.FindByIdAsync(request.Id);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }
}

/// <summary>
/// Handler for searching customers by name
/// </summary>
public class SearchCustomersByNameQueryHandler : IRequestHandler<SearchCustomersByNameQuery, IEnumerable<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchCustomersByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(SearchCustomersByNameQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.FindByNameAsync(request.FirstName, request.LastName);
        return customer != null ? new[] { _mapper.Map<CustomerDto>(customer) } : Enumerable.Empty<CustomerDto>();
    }
}