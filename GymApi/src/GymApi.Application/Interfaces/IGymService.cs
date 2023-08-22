using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces
{
    public interface IGymService
    {
        Task<Gimnasio> ObtenerGym(int IdGym);
        Task<Gimnasio> Crear(Gimnasio entidad, Stream Foto = null, string carpetaDestino = "", string nombreFoto = "");
        Task<Gimnasio> GuardarPerfil(Gimnasio entidad, Stream Logo = null, string carpetaDestino = "", string nombreLogo = "");
        Task<bool> CambiarClave(int IdGym, string claveActual, string claveNueva);
        Task<bool> EliminarPerfil(int IdGym, string claveActual);
    }
}