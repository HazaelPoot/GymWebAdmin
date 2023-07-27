using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface ICorreoService
    {
        void RegisterGymEmail(Gimnasio entity);
        void SuscriptionEmail(Suscripcion entity);
        void NewpayEmail(Suscripcion entity);
    }
}