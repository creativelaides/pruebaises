using Microsoft.AspNetCore.Mvc.Authorization;
using Scalar.AspNetCore;
using TarifasElectricas.Application;
using TarifasElectricas.Api.Middlewares;
using TarifasElectricas.Identity;
using TarifasElectricas.Identity.Models;
using TarifasElectricas.Identity.Seed;
using TarifasElectricas.Infrastructure;
using TarifasElectricas.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddScoped<ApiExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.EnablePersistentAuthentication();
    });
}

app.UseApiExceptionHandling();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<AppUser>();

await IdentitySeeder.SeedAsync(app.Services, app.Configuration);

app.Run();
