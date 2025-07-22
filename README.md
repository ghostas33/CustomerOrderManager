# Customer Order Management API

A .NET 8 Web API for managing customers and their orders, built with clean architecture principles.

## Features

- Customer management (CRUD operations)
- Order management with items
- Product catalog
- RESTful API design
- Entity Framework Core with SQL Server
- CQRS pattern with MediatR
- Repository and Unit of Work patterns
- Input validation with FluentValidation

## Architecture

This project follows Clean Architecture principles with:
- Domain-driven design
- CQRS pattern for separating read/write operations
- Repository pattern with Unit of Work
- Dependency injection
- Entity Framework Core for data persistence

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer
- `GET /api/customers/{id}/orders` - Get customer orders by date range

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

## Business Rules

- Customer names are required
- Orders must have at least one item
- Product prices must be positive
- Order totals are calculated automatically
- Customers with existing orders cannot be deleted

## Database Schema

The application uses Entity Framework Core with the following main entities:
- **Customer**: Basic customer information
- **Order**: Customer orders with date and total price
- **OrderItem**: Individual items within an order
- **Product**: Product catalog with name and price

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MediatR (CQRS)
- AutoMapper
- FluentValidation

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB is fine for development)

### Running the application
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start the application: `dotnet run`



## Development Notes

- Uses in-memory database for testing
- Implements basic validation rules
- No authentication required for this demo
- Follows RESTful API conventions

## TODO

- [ ] Add unit tests
- [ ] Implement comprehensive testing strategy
- [ ] Add pagination for large datasets
- [ ] Implement caching for frequently accessed data
- [ ] Add more comprehensive error handling
- [ ] Consider adding authentication in future versions