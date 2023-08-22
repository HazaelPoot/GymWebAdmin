using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces
{
    public interface IAccesoService
    {
        Task<Gimnasio> Authentication(string correo, string password);
        Task<Gimnasio> GetGymAuth (int IdGimnasio);
        Task<bool> GenerateClaims(Gimnasio user);
    }
}