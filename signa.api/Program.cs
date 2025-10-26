using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Signa.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container
builder.Services.AddControllers();

// Add Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Signa API",
        Version = "v1",
        Description = "API for managing digital advertising tablets (Signa Platform)."
    });
});

var app = builder.Build();

// Enable Swagger UI only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Signa API v1");
        c.RoutePrefix = string.Empty; // makes Swagger available at root "/"
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();