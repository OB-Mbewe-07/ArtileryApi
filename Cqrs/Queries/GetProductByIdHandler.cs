using System.Data;
using Dapper;

namespace Cqrs.Queries;

public class GetProductByIdHandler
{
    private readonly IDbConnection _db;

    public GetProductByIdHandler(IDbConnection db)
    {
        _db = db;
    }

    public async Task<ProductDetailsDto?> Handler(GetProductByIdQuery query)
    {
       const string sql = """
            SELECT 
                Id,
                Name,
                Description,
                Price,
                StockQuanitity,
                CASE WHEN StockQuanitity > 0 
                     THEN 'In Stock' 
                     ELSE 'Out of Stock' 
                END as StockStatus,
                strftime('%d %m %Y', CreatedAt) as CreatedAt
            FROM Products
            WHERE Id = @Id
            """;

        return await _db.QueryFirstOrDefaultAsync<ProductDetailsDto>(sql, new { query.Id });
    }
}