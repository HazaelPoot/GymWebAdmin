using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface IServicioService
    {
        Task<List<Servicio>> Lista(int IdGym);
        Task<Servicio> GetById (int idServicio);
        Task<Servicio> Crear(int idGym, Servicio entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<Servicio> Editar(Servicio entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<bool> Eliminar(int idServicio);
    }
}