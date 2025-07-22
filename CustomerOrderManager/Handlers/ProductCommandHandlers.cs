using MediatR;
using AutoMapper;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Models.Entities;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for creating a new product
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var savedProduct = await _unitOfWork.Products.SaveAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(savedProduct);
    }
}

/// <summary>
/// Handler for updating an existing product
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _unitOfWork.Products.FindByIdAsync(request.Id);
        if (existingProduct == null)
        {
            throw new ArgumentException($"Product with ID {request.Id} not found");
        }

        existingProduct.Name = request.Name;
        existingProduct.Price = request.Price;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _unitOfWork.Products.SaveAsync(existingProduct);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(updatedProduct);
    }
}

/// <summary>
/// Handler for deleting a product
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.FindByIdAsync(request.Id);
        if (product == null)
        {
            return false;
        }

        await _unitOfWork.Products.DeleteByIdAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}