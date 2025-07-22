using MediatR;
using AutoMapper;
using CustomerOrderManager.Queries;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for getting all products
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.FindAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}

/// <summary>
/// Handler for getting a product by ID
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.FindByIdAsync(request.Id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }
}