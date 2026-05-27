using MediatR;
using Cqrs.Data;
using Cqrs.Commands;

namespace Cqrs.Behaviors;

public class VarianceCheckBehavior : IPipelineBehavior<SubmitStockCountCommand, SubmitStockCountResult>
{
    private readonly AppDbContext _db;

    private const decimal FinancialThreshold = 500m;
    private const decimal PercentageThreshold = 0.10m; 

    public VarianceCheckBehavior(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SubmitStockCountResult> Handle(
        SubmitStockCountCommand request,
        RequestHandlerDelegate<SubmitStockCountResult> next,
        CancellationToken cancellationToken)
    {
        var product = await _db.Products.FindAsync(request.ProductId);

        if (product is null)
        {
            throw new ArgumentException($"Product with ID {request.ProductId} not found.");
        }
            
        var variance = Math.Abs(request.ExpectedStock - request.SubmittedStock);
        var variancePercentage = (decimal)variance / request.ExpectedStock;
        var varianceValue = variance * product.Price;

        var exceedsPercentage = variancePercentage > PercentageThreshold;
        var exceedsFinancial = varianceValue > FinancialThreshold;

        if (exceedsPercentage || exceedsFinancial)
        {
            var reasons = new List<string>();

            if (exceedsPercentage)
            {
                reasons.Add($"Variance of {variancePercentage:P0} exceeds the 10% threshold");
            }
                
            if (exceedsFinancial)
            {
                reasons.Add($"Financial impact of {varianceValue:C} exceeds the £500 threshold");
            }


            return new SubmitStockCountResult
            {
                AuditId = 0,
                Status = "PendingReview",
                FlagReason = string.Join(" and ", reasons),
                Variance = request.ExpectedStock - request.SubmittedStock,
                VarianceValue = varianceValue
            };
        }
        return await next();
    }
}