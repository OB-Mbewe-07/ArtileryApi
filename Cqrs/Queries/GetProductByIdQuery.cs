namespace Cqrs.Queries;

public class GetProductByIdQuery
{
    public int Id { get; set; }
}

public class ProductDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuanitity { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}