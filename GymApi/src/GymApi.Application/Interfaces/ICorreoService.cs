using GymApi.Domain.Entities;

namespace GymApi.Application.Interfaces
{
    public interface ICorreoService
    {
        Task RegisterGymEmail(Gimnasio entity);
        Task SuscriptionEmail(Suscripcion entity);
        Task NewpayEmail(Suscripcion entity);
    }
}