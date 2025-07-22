using MediatR;
using AutoMapper;
using CustomerOrderManager.Commands;
using CustomerOrderManager.Model.DTO;
using CustomerOrderManager.Models.Entities;
using CustomerOrderManager.Repository;

namespace CustomerOrderManager.Handlers;

/// <summary>
/// Handler for creating a new customer
/// </summary>
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Customer>(request);
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        var savedCustomer = await _unitOfWork.Customers.SaveAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(savedCustomer);
    }
}

/// <summary>
/// Handler for updating an existing customer
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingCustomer = await _unitOfWork.Customers.FindByIdAsync(request.Id);
        if (existingCustomer == null)
            throw new ArgumentException($"Customer with ID {request.Id} not found");

        _mapper.Map(request, existingCustomer);
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        var updatedCustomer = await _unitOfWork.Customers.SaveAsync(existingCustomer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(updatedCustomer);
    }
}

/// <summary>
/// Handler for deleting a customer
/// </summary>
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.FindByIdAsync(request.Id);
        if (customer == null)
            return false;

        await _unitOfWork.Customers.DeleteByIdAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}