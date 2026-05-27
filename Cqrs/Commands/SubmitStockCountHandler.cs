using MediatR;
using Cqrs.Data;
using Cqrs.Models;

namespace Cqrs.Commands;

public class SubmitStockCountHandler : IRequestHandler<SubmitStockCountCommand, SubmitStockCountResult>
{
    private readonly AppDbContext _db;

    public SubmitStockCountHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SubmitStockCountResult> Handle(
        SubmitStockCountCommand command,
        CancellationToken cancellationToken
    )
    {
        var variance = command.ExpectedStock - command.SubmittedStock;
        var product = await _db.Products.FindAsync(command.ProductId);

        if (product is null)
        {
            throw new ArgumentException($"Product with ID {command.ProductId} not found.");
        }

        var varianceValue = Math.Abs(variance) * product.Price;

        var audit = new StockAudit
        {
            ProductId = command.ProductId,
            ExpectedStock = command.ExpectedStock,
            SubmittedStock = command.SubmittedStock,
            Variance = variance,
            VarianceValue = varianceValue,
            Status = "Approved",
            FlagReason = null,
            CreatedAt = DateTime.UtcNow
        };

        _db.StockAudits.Add(audit);
        await _db.SaveChangesAsync(cancellationToken);

        return new SubmitStockCountResult
        {
            AuditId = audit.Id,
            Status = audit.Status,
            FlagReason = audit.FlagReason,
            Variance = audit.Variance,
            VarianceValue = audit.VarianceValue
        };
    }
}