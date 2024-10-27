using MinimalApi.Dominio.Entidades;
using MinimalApi.DTOs;

namespace minimal_api2.Dominio.Interfaces;

public interface IAdministradorServico{
    Administrador? Login(LoginDTO loginDTO);
}
