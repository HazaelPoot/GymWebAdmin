using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Application.Services
{
    public class SuscripcionService : ISuscripcionService
    {
        private readonly IGenericRepository<Suscripcion> _repository;
        private readonly IServicioService _servicioService;
        private readonly IMiembroService _miembroService;
        private readonly ICorreoService _correoService;
        private readonly IGymService _gymService;

        public SuscripcionService(IGenericRepository<Suscripcion> repository, IGymService gymService, IMiembroService miembroService, IServicioService servicioService, ICorreoService correoService)
        {
            _repository = repository;
            _gymService = gymService;
            _correoService = correoService;
            _miembroService = miembroService;
            _servicioService = servicioService;
        }

        public async Task<List<Suscripcion>> Lista(int idGimnasio)
        {
            Gimnasio gymExist = await _gymService.ObtenerGym(idGimnasio);
            if (gymExist == null)
                throw new TaskCanceledException($"No existe un Gimnasio con el ID {idGimnasio}");

            try
            {
                IQueryable<Suscripcion> query = await _repository.Consult(h => h.IdServicioNavigation.IdGimnasio == idGimnasio);
                var result = query.Include(m => m.IdMiembroNavigation.IdUsuarioNavigation).Include(s => s.IdServicioNavigation).ToList();
                foreach (var item in result)
                {
                    await VerificarPago(item);
                }
                
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Suscripcion> GetById(int idSuscripcion)
        {
            Suscripcion suscripcionExist = await _repository.Obtain(m => m.IdInscripcion == idSuscripcion);
            if(suscripcionExist == null)
                throw new TaskCanceledException($"No existe una Suscripcion con el ID {idSuscripcion}");

            try
            {
                IQueryable<Suscripcion> query = await _repository.Consult(m => m.IdInscripcion == idSuscripcion);
                Suscripcion result = query.Include(g => g.IdServicioNavigation).Include(u => u.IdMiembroNavigation.IdUsuarioNavigation).First();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Suscripcion> VerificarPago(Suscripcion entidad)
        {
            try
            {
                IQueryable<Suscripcion> querySuscrip = await _repository.Consult(s => s.IdInscripcion == entidad.IdInscripcion);
                Suscripcion suscrip_editar = querySuscrip.First();
                suscrip_editar.ClasePago = "sinClase";

                DateTime fechaPago;
                DateTime.TryParse(suscrip_editar.FechaPago, out fechaPago);

                if (fechaPago < DateTime.Now && suscrip_editar.Pago == 0)
                    suscrip_editar.ClasePago = "noPago";

                if (fechaPago < DateTime.Now && suscrip_editar.Pago == 1)
                {
                    suscrip_editar.ClasePago = "sinClase";
                    suscrip_editar.Pago = 0;
                    suscrip_editar.FechaPago = fechaPago.AddMonths(1).ToString("dd/MM/yyyy");
                }

                bool response = await _repository.Edit(suscrip_editar);
                if (!response)
                    throw new TaskCanceledException("No se pudo verificar el pago");

                Suscripcion susEditado = querySuscrip.Include(g => g.IdServicioNavigation).Include(u => u.IdMiembroNavigation.IdUsuarioNavigation).First();

                return susEditado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Suscripcion> Crear(Suscripcion entidad)
        {
            Miembro miembro = await _miembroService.GetById(entidad.IdMiembro);
            Servicio servicio = await _servicioService.GetById(entidad.IdServicio);

            if (miembro.IdGimnasio != servicio.IdGimnasio)
                throw new TaskCanceledException($"El servicio al que intentas suscribirte no pertenece a este Gimnasio");

            Suscripcion susExist = await _repository.Obtain(s => s.IdMiembro == entidad.IdMiembro && s.IdServicio == entidad.IdServicio);
            if (susExist != null)
                throw new TaskCanceledException($"Este usuario ya esta suscrito a este servicio");

            try
            {
                entidad.ClaveAcceso = Guid.NewGuid().ToString("N").Substring(0, 11);
                entidad.FechaPago = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                entidad.Pago = 0;

                Suscripcion susCreado = await _repository.Create(entidad);

                if (susCreado.IdInscripcion == 0)
                    throw new TaskCanceledException("El Usuario no se puedo suscribir a este servicio");

                IQueryable<Suscripcion> query = await _repository.Consult(h => h.IdInscripcion == susCreado.IdInscripcion);
                susCreado = query.Include(g => g.IdServicioNavigation).Include(u => u.IdMiembroNavigation.IdUsuarioNavigation).First();

                _correoService.SuscriptionEmail(susCreado);

                return susCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Suscripcion> CambiarPago(Suscripcion entidad)
        {
            Suscripcion susExist = await _repository.Obtain(s => s.IdInscripcion == entidad.IdInscripcion);
            if (susExist is null)
                throw new TaskCanceledException($"Esta suscripción no existe");

            try
            {
                IQueryable<Suscripcion> querySuscrip = await _repository.Consult(s => s.IdInscripcion == entidad.IdInscripcion);
                Suscripcion suscrip_editar = querySuscrip.First();
                suscrip_editar.Pago = entidad.Pago;

                bool response = await _repository.Edit(suscrip_editar);
                if (!response)
                    throw new TaskCanceledException("No se pudo asignar el pago a la suscripción");

                Suscripcion susEditado = querySuscrip.Include(g => g.IdServicioNavigation).Include(u => u.IdMiembroNavigation.IdUsuarioNavigation).First();

                return susEditado;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Suscripcion> Validar(int idGym, string parametro)
        {
            Suscripcion suscripcionExist = await _repository.Obtain(s => s.IdMiembroNavigation.IdUsuarioNavigation.Nombre == parametro && s.IdServicioNavigation.IdGimnasio == idGym || s.ClaveAcceso == parametro && s.IdServicioNavigation.IdGimnasio == idGym);
            if(suscripcionExist == null)
                throw new TaskCanceledException($"No se encontro una Suscripcion los datos proporcionados, incluye mayusculas y acentos si es necesario");

            try
            {
                IQueryable<Suscripcion> query = await _repository.Consult(m => m.IdMiembroNavigation.IdUsuarioNavigation.Nombre == parametro  || m.ClaveAcceso == parametro);
                Suscripcion result = query.Include(g => g.IdServicioNavigation).Include(u => u.IdMiembroNavigation.IdUsuarioNavigation).First();

                DateTime fechaPago;
                DateTime.TryParse(result.FechaPago, out fechaPago);

                if(fechaPago < DateTime.Now && result.Pago == 0)
                    throw new TaskCanceledException($"El miembro no ha pagado su suscripcion o presenta un adeudo");
                
                return result;
            }
            
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idSuscripcion)
        {
            Suscripcion suscrEncontrado = await _repository.Obtain(s => s.IdInscripcion == idSuscripcion);
            if(suscrEncontrado is null)
                throw new TaskCanceledException("La suscripcion no existe");

            try
            {   
                bool response = await _repository.Eliminate(suscrEncontrado);
                if(!response)
                    throw new TaskCanceledException("No se puedo eliminar la suscripción");
                
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}