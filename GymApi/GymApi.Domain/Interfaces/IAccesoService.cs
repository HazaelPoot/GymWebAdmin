using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface IAccesoService
    {
        Task<Gimnasio> Authentication(string correo, string password);
        Task<Gimnasio> GetUserAuth (int IdGimnasio);
        Task<bool> GenerateClaims(DtoGimnasio user);
    }
}