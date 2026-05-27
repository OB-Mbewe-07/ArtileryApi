namespace Cqrs.Models;

public class StockAudit
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ExpectedStock { get; set; }
    public int SubmittedStock { get; set; }
    public int Variance { get; set; }
    public decimal VarianceValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FlagReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public Product? Product { get; set; }
}