using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api2.Dominio.Interfaces;
using minimal_api2.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

// Adiciona o DbContexto com a string de conexão do appsettings
builder.Services.AddDbContext<DbContexto>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConexaoPadrao");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) =>
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

app.Run();
