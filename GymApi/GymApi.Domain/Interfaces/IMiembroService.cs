using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface IMiembroService
    {
        Task<List<Miembro>> Lista(int idGimnasio);
        Task<Miembro> GetById(int idMiembro);
        Task<Miembro> Crear(int idGimnasio, int idUsuario);
        Task<Miembro> Agregar(int idGimnasio, Usuario entidad);
        Task<bool> Eliminar(int idMiembro);
    }
}