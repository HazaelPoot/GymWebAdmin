using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;

namespace GymApi.Application.Services
{
    public class GymService : IGymService
    {
        private readonly IGenericRepository<Gimnasio> _repository;
        private readonly IFirebaseService _fireBaseService;
        private readonly IUtilityService _utilityService;
        private readonly ICorreoService _correoService;

        public GymService(IGenericRepository<Gimnasio> repository, IFirebaseService firebaseService, IUtilityService utilityService, ICorreoService correoService)
        {
            _repository = repository;
            _correoService = correoService;
            _utilityService = utilityService;
            _fireBaseService = firebaseService;
        }

        public async Task<Gimnasio> ObtenerGym(int IdGym)
        {
            Gimnasio gym_encontrado = await _repository.Obtain(u => u.IdGimnasio == IdGym);
            return gym_encontrado;
        }

        public async Task<Gimnasio> Crear(Gimnasio entidad, Stream Foto = null, string carpetaDestino = "", string nombreFoto = "")
        {
            Gimnasio gymExist = await _repository.Obtain(u => u.Correo == entidad.Correo);
            if(gymExist != null)
                throw new TaskCanceledException($"Ya existe un Gimnasio con el correo {entidad.Correo}");
            
            try
            {
                entidad.Passw = _utilityService.EncryptMD5(entidad.Passw);
                entidad.NombreFoto = nombreFoto;

                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, nombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                Gimnasio gymCreado = await _repository.Create(entidad);
                if(gymCreado.IdGimnasio == 0)
                    throw new TaskCanceledException("No se pudo registrar el Gimnasio");
                
                IQueryable<Gimnasio> query = await _repository.Consult(u => u.IdGimnasio == gymCreado.IdGimnasio);
                gymCreado = query.First();

                _correoService.RegisterGymEmail(gymCreado);

                return gymCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Gimnasio> GuardarPerfil(Gimnasio entidad, Stream Logo = null, string carpetaDestino = "", string nombreLogo = "")
        {
            Gimnasio gym_exist = await _repository.Obtain(u => u.Correo == entidad.Correo && u.IdGimnasio != entidad.IdGimnasio);
            if(gym_exist != null)
                throw new TaskCanceledException($"Ya existe un Gimnasio usando el correo {entidad.Correo}");

            try
            {
                Gimnasio gym_encontrado = await _repository.Obtain(u => u.IdGimnasio == entidad.IdGimnasio);
                
                gym_encontrado.Nombre = entidad.Nombre;
                gym_encontrado.Contacto = entidad.Contacto;
                gym_encontrado.Direccion = entidad.Direccion;
                gym_encontrado.Ciudad = entidad.Ciudad;
                gym_encontrado.Correo = entidad.Correo;
                gym_encontrado.NombreFoto = gym_encontrado.NombreFoto == "" ? nombreLogo: gym_encontrado.NombreFoto;
                
                if(Logo != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Logo, carpetaDestino, gym_encontrado.NombreFoto);
                    gym_encontrado.UrlImagen = urlFoto;
                }

                await _repository.Edit(gym_encontrado);
                return gym_encontrado;
            }
            catch 
            {
                throw;
            }
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
                Gimnasio gym_encontrado = await _repository.Obtain(u => u.IdGimnasio == IdGym);
                string claveGym = _utilityService.DesencryptMD5(gym_encontrado.Passw);

                if(gym_encontrado is null)
                    throw new TaskCanceledException("El Gimnasio no existe");
                
                if(claveActual != claveGym)
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                bool response = await _repository.Eliminate(gym_encontrado);

                return response;
                
            }
            catch
            {
                throw;
            }
        }
    }
}