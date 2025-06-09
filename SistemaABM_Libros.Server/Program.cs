using Microsoft.EntityFrameworkCore;
using SistemaABM_Libros_Data.Mapper;
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
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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
