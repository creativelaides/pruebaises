using TarifasElectricas.Application;
using TarifasElectricas.Infrastructure;
// using TarifasElectricas.Api; // For DependencyInjectionApi - removed for now
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Explicitly configure SwaggerGen
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tarifas Electricas API", Version = "v1" });
});

builder.Services.AddControllers(); // Still needed for controllers

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Exposes the OpenAPI JSON endpoint
    app.MapScalarApiReference(); // Serves the Scalar UI
}

app.UseHttpsRedirection();

app.UseRouting(); // Ensure routing is configured before authorization and endpoint mapping

app.UseAuthorization();

app.MapControllers(); // Maps controller routes

app.Run();
