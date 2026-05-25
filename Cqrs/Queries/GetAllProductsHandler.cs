using Cqrs.Data;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Queries;

public class ProductSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string StockStatus { get; set; } = string.Empty;
}

public class GetAllProductHandler
{
    private readonly AppDbContext _db;
    public GetAllProductHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductSummaryDto>> handler()
    {
        return await _db.Products.AsNoTracking().Select(p => new ProductSummaryDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockStatus = p.StockQuanitity > 0 ? "In Stock" : "Out of. stock"
        })
        .ToListAsync();
    }
}