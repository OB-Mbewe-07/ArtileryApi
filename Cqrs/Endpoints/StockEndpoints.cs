using MediatR;
using Cqrs.Commands;

namespace Cqrs.Endpoints;

public static class StockEndpoints
{
    public static void MapStockEndpoints(this WebApplication app)
    {
        app.MapPost("/api/stock/count", async (
            SubmitStockCountCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });
    }
}