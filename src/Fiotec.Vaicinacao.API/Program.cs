using Fiotec.Vaicinacao.API.Data;
using Fiotec.Vaicinacao.API.Repositories.Interfaces;
using Fiotec.Vaicinacao.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Fiotec.Vaicinacao.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

//Inje��o de depend�ncia do Banco de Dados
builder.Services.AddDbContext<SqlContext>(opt 
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("FiotecDB")));

//Inje��es de depend�ncia
builder.Services.AddTransient<ISolicitanteRepository, SolicitanteRepository>();
builder.Services.AddTransient<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddHttpClient<RelatorioController>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.Run();
