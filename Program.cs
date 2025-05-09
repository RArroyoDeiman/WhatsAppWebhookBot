using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios de controladores con soporte para JSON
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers(); // Asegura que los controladores se mapeen

app.Run();
