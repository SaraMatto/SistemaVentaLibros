using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_Repository.Sevice;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios para la base de datos (DbContext) 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BDSistemaLibrosContext>(options =>
    options.UseSqlServer(connectionString));
// Add services to the container.


builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IServiceUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServiceCategoria, ServiceCategoria>();
builder.Services.AddScoped<IServiceSubcategoria, ServiceSubcategoria>();
builder.Services.AddScoped<IServicePedido, ServicePedido>();
builder.Services.AddScoped<IServiceLibro, ServiceLibro>();
builder.Services.AddScoped<IServiceDetallePedido, ServiceDetallePedido>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.WithOrigins("https://localhost:62614") // origen exacto de tu front
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});


var app = builder.Build();
// Habilitar CORS usando la política definida
app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Img")),
    RequestPath = "/img"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
