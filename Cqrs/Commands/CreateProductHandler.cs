using Cqrs.Data;
using Cqrs.Models;

namespace Cqrs.Commands;

public class CreateProductHandler
{
    private readonly AppDbContext _db;

    public CreateProductHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CreateProductResult> Handler(CreateProductCommand command)
    {
        if (command.Price <= 0)
        {
            throw new ArgumentException("Price should be bigger than 0");
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Required Product name");
        }

        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            StockQuanitity = command.StockQuanitity,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return new CreateProductResult
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}