using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> GetById (int idUsuario);
        Task<Usuario> Crear(Usuario entidad);
        Task<Usuario> Editar(Usuario entidad);
        Task<bool> Eliminar(int idUsuario);
    }
}