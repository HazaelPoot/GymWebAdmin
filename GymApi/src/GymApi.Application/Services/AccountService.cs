using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using GymApi.Application.Interfaces;

namespace GymApi.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericSqlRepository<Gimnasio> _repository;
        private readonly ISuscripcionService _suscripcionService;
        private readonly IServicioService _servicioService;
        private readonly IFirebaseService _firebaseService;
        private readonly IUtilityService _utilityService;
        private readonly IMiembroService _miembroService;
        private readonly IHorarioService _horarioService;

        public AccountService(IGenericSqlRepository<Gimnasio> repository, IUtilityService utilityService, ISuscripcionService suscripcionService, IMiembroService miembroService, IHorarioService horarioService, IServicioService servicioService, IFirebaseService firebaseService)
        {
            _repository = repository;
            _miembroService = miembroService;
            _horarioService = horarioService;
            _utilityService = utilityService;
            _servicioService = servicioService;
            _firebaseService = firebaseService;
            _suscripcionService = suscripcionService;
        }

        public async Task<bool> CambiarClave(int IdGym, string claveActual, string claveNueva)
        {
            try
            {
                Gimnasio gym_encontrado = await _repository.Obtain(u => u.IdGimnasio == IdGym);
                string claveGym = _utilityService.DesencryptMD5(gym_encontrado.Passw);

                if(gym_encontrado is null)
                    throw new TaskCanceledException("El Gimnasio no existe");
                
                if(claveGym != claveActual)
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                gym_encontrado.Passw = _utilityService.EncryptMD5(claveNueva);

                bool response = await _repository.Edit(gym_encontrado);

                return response;
                
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EliminarPerfil(int IdGym, string claveActual)
        {
            try
            {
                Gimnasio gym_encontrado = await _repository.Obtain(u => u.IdGimnasio == IdGym)
                    ?? throw new TaskCanceledException("El Gimnasio no existe");
                
                string nombreFoto = gym_encontrado.NombreFoto;
                string claveGym = _utilityService.DesencryptMD5(gym_encontrado.Passw);
                
                if(claveActual != claveGym)
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                // 1. Suscripciones
                await _suscripcionService.EliminarTodo(IdGym);
                // 2. Miembros
                await _miembroService.EliminarTodo(IdGym);
                // 3. Horarios
                await _horarioService.EliminarTodo(IdGym);
                // 4. Servicios => Imagenes
                await _servicioService.EliminarTodo(IdGym);

                bool response = await _repository.Eliminate(gym_encontrado);
                if(response)
                    await _firebaseService.DeleteStorage("Logo_Gym", nombreFoto);

                return response;

                //PENDIENTE AFINAR DETALLES PERO TODO BIEN..!!
                
            }
            catch
            {
                throw;
            }
        }
    }
}