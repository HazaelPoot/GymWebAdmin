using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Application.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IGenericRepository<Gimnasio> _gymRepository;
        private readonly IGenericRepository<Servicio> _repository;
        private readonly IFirebaseService _fireBaseService;

        public ServicioService(IGenericRepository<Gimnasio> gymRepository, IGenericRepository<Servicio> repository, IFirebaseService fireBaseService)
        {
            _repository = repository;
            _gymRepository = gymRepository;
            _fireBaseService = fireBaseService;
        }

        public async Task<List<Servicio>> Lista(int IdGym)
        {
            Gimnasio gymExist = await _gymRepository.Obtain(g => g.IdGimnasio == IdGym);
            if(gymExist is null)
                throw new TaskCanceledException($"No existe un Gimnasio con el Id {IdGym}");
                
            try
            {
                IQueryable<Servicio> query = await _repository.Consult(s => s.IdGimnasio == IdGym);
                return query.Include(g => g.IdGimnasioNavigation).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Servicio> GetById(int idServicio)
        {
            Servicio exist = await _repository.Obtain(s => s.IdServicio == idServicio);
            if(exist == null)
                throw new TaskCanceledException($"No existe en Servicio {idServicio}");

            try
            {
                IQueryable<Servicio> query = await _repository.Consult(s => s.IdServicio == idServicio);
                Servicio result = query.Include(g => g.IdGimnasioNavigation).FirstOrDefault();
                
                return result;
            }
            catch
            {
                
                throw;
            }
        }

        public async Task<Servicio> Crear(int idGym, Servicio entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            IQueryable<Servicio> serviciosEnGimnasio = await _repository.Consult(s => s.IdGimnasio == idGym && s.Nombre == entidad.Nombre);
            if(serviciosEnGimnasio.Any())
                throw new TaskCanceledException($"Ya existe un servicio con el nombre {entidad.Nombre} en este Gimnasio");

            try
            {
                entidad.NombreFoto = NombreFoto;
                entidad.IdGimnasio = idGym;
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                Servicio servicioCreado = await _repository.Create(entidad);
                if(servicioCreado.IdGimnasio == 0)
                    throw new TaskCanceledException("No se pudo registrar el Servicio");
                
                IQueryable<Servicio> query = await _repository.Consult(s => s.IdServicio == servicioCreado.IdServicio);
                servicioCreado = query.Include(g => g.IdGimnasioNavigation).First();

                return servicioCreado;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Servicio> Editar(Servicio entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            IQueryable<Servicio> serviciosEnGimnasio = await _repository.Consult(s => s.IdGimnasio == entidad.IdGimnasio && s.Nombre == entidad.Nombre && s.IdServicio != entidad.IdServicio);
            if(serviciosEnGimnasio.Any())
                throw new TaskCanceledException($"Ya existe un servicio con el nombre {entidad.Nombre}");
            
            try
            {
                IQueryable<Servicio> queryServicio = await _repository.Consult(s => s.IdServicio == entidad.IdServicio);
                Servicio servicio_editar = queryServicio.First();
                servicio_editar.Nombre = entidad.Nombre;
                servicio_editar.Costo = entidad.Costo;
                servicio_editar.Detalles = entidad.Detalles;

                if(servicio_editar.NombreFoto == "")
                    servicio_editar.NombreFoto = NombreFoto;
                
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    servicio_editar.UrlImagen = urlFoto;
                }

                bool response = await _repository.Edit(servicio_editar);
                if(!response)
                    throw new TaskCanceledException("No se pudo moficiar el servicio");
                
                Servicio servicio_editado = queryServicio.Include(g => g.IdGimnasioNavigation).First();

                return servicio_editado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idServicio)
        {
            try
            {
                Servicio servicioEncontrado = await _repository.Obtain(s => s.IdServicio == idServicio);
                
                if(servicioEncontrado == null)
                    throw new TaskCanceledException("El Servicio no existe");
                
                string nombreFoto = servicioEncontrado.NombreFoto;
                bool response = await _repository.Eliminate(servicioEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Gym_Servicios", nombreFoto);
                
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}