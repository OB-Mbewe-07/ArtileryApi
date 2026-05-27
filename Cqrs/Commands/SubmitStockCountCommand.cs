

using MediatR;

namespace Cqrs.Commands;

public record SubmitStockCountCommand(int ProductId, int ExpectedStock, int SubmittedStock): IRequest<SubmitStockCountResult>;

public class SubmitStockCountResult
{
    public int AuditId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FlagReason { get; set; }
    public int Variance { get; set; }
    public decimal VarianceValue { get; set; }
}