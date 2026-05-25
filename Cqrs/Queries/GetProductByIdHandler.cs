using Cqrs.Data;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Queries;

public class GetProductByIdHandler
{
    private readonly AppDbContext _db;

    public GetProductByIdHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProductDetailsDto?> Handler(GetProductByIdQuery query)
    {
        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.Id);

        if (product is null) return null;

        return new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuanitity = product.StockQuanitity,
            StockStatus = product.StockQuanitity > 0 ? "In Stock" : "Out of Stock",
            CreatedAt = product.CreatedAt.ToString("dd MMM yyyy")
        };
    }
}