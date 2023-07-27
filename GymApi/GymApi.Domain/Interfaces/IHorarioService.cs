using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface IHorarioService
    {
        Task<List<Horario>> Lista(int idGimnasio);
        Task<Horario> GetById (int idHorario);
        Task<Horario> Crear(int idGimnasio, Horario entidad);
        Task<Horario> Editar(Horario entidad);
        Task<bool> Eliminar(int idHorario);
    }
}