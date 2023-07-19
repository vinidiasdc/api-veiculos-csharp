using apiVeiculos.Infraestrutura;
using apiVeiculos.Mapeamento;
using apiVeiculos.Repositorios;
using apiVeiculos.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var stringDeConexao = builder.Configuration.GetConnectionString("DefaultConection");

builder.Services.AddDbContext<ConexaoContext>(options => options.UseSqlServer(stringDeConexao, b => b.MigrationsAssembly(typeof(ConexaoContext).Assembly.FullName)));

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();

builder.Services.AddAutoMapper(typeof(EntidadeParaDTOConversor));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
