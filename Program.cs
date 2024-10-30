using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api2.Dominio.Interfaces;
using minimal_api2.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;
using minimal_api2.Dominio.ModelViews;
using MinimalApi.Dominio.Entidades;
#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o DbContexto com a string de conexão do appsettings
builder.Services.AddDbContext<DbContexto>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConexaoPadrao");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();
#endregion

#region  Home

app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO)!= null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});


// Endpoint para obter todos os administradores
app.MapGet("/administradores", async (DbContexto db) =>
{
    var administradores = await db.Administradores.ToListAsync();
    return Results.Ok(administradores);
});
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody]VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo{
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});
#endregion
#region APP
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
