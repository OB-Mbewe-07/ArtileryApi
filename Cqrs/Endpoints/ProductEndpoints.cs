using Cqrs.Commands;
using Cqrs.Queries;

namespace Cqrs.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapPost("/api/products", async (CreateProductCommand command, CreateProductHandler handler) =>
        {
            var result = await handler.Handler(command);
            return Results.Created($"/api/products/{result.Id}", result);
        });

        app.MapGet("/api/products/{id}", async (int id, GetProductByIdHandler handler) =>
        {
            var result = await handler.Handler(new GetProductByIdQuery { Id = id });
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapGet("/api/products", async (GetAllProductHandler handler) =>
        {
            var result = await handler.handler();
            return Results.Ok(result);
        });
    }
}