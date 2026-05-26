using System.Data;
using Dapper;

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
    private readonly IDbConnection _db;
    public GetAllProductHandler(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductSummaryDto>> handler()
    {
       const string sql = """
            SELECT 
                Id,
                Name,
                Price,
                CASE WHEN StockQuanitity > 0 
                     THEN 'In Stock' 
                     ELSE 'Out of Stock' 
                END as StockStatus
            FROM Products
            """;
        return await _db.QueryAsync<ProductSummaryDto>(sql);
    }
}