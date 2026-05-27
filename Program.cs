using Microsoft.EntityFrameworkCore;
using Cqrs.Data;
using Cqrs.Commands;
using Cqrs.Queries;
using Cqrs.Endpoints;
using System.Data;
using Microsoft.Data.Sqlite;
using MediatR;
using Cqrs.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<GetAllProductHandler>();
builder.Services.AddScoped<IDbConnection>(sp => new SqliteConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPipelineBehavior<SubmitStockCountCommand, SubmitStockCountResult>, VarianceCheckBehavior>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapProductEndpoints();
app.MapStockEndpoints();

app.Run();