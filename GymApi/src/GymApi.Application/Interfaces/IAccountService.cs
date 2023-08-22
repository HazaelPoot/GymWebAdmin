namespace GymApi.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CambiarClave(int IdGym, string claveActual, string claveNueva);
        Task<bool> EliminarPerfil(int IdGym, string claveActual);
    }
}