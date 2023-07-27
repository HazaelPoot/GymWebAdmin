using GymApi.Domain.Entities;

namespace GymApi.Domain.Interfaces
{
    public interface ISuscripcionService
    {
        Task<List<Suscripcion>> Lista(int idGimnasio);
        Task<Suscripcion> GetById (int idSuscripcion);
        Task<Suscripcion> Crear(Suscripcion entidad);
        Task<Suscripcion> VerificarPago(Suscripcion entidad);
        Task<Suscripcion> CambiarPago(Suscripcion entidad);
        Task<Suscripcion> Validar(int idGym, string parametro);
        Task<bool> Eliminar(int idSuscripcion);
    }
}