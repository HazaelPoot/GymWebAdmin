using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Application.Services
{
    public class MiembroService : IMiembroService
    {
        private readonly IGenericRepository<Miembro> _repository;
        private readonly IUsuarioService _usuarioService;
        private readonly IGymService _gymService;

        public MiembroService(IGenericRepository<Miembro> repository, IGymService gymService, IUsuarioService usuarioService)
        {
            _repository = repository;
            _gymService = gymService;
            _usuarioService = usuarioService;
        }

        public async Task<List<Miembro>> Lista(int idGimnasio)
        {
            Gimnasio gymExist = await _gymService.ObtenerGym(idGimnasio);
            if (gymExist == null)
                throw new TaskCanceledException($"No existe un Gimnasio con el ID {idGimnasio}");

            try
            {
                IQueryable<Miembro> query = await _repository.Consult(h => h.IdGimnasio == idGimnasio);
                return query.Include(u => u.IdUsuarioNavigation).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Miembro> GetById(int idMiembro)
        {
            Miembro miemExist = await _repository.Obtain(m => m.IdMiembro == idMiembro);
            if(miemExist == null)
                throw new TaskCanceledException($"No existe un Horario con el ID {idMiembro}");

            try
            {
                IQueryable<Miembro> query = await _repository.Consult(m => m.IdMiembro == idMiembro);
                Miembro result = query.Include(g => g.IdGimnasioNavigation).Include(u => u.IdUsuarioNavigation).First();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Miembro> Crear(int idGimnasio, int idUsuario)
        {
            try
            {
                Miembro entidad = new Miembro
                {
                    IdUsuario = idUsuario,
                    IdGimnasio = idGimnasio
                };

                Miembro miembroExist = await _repository.Obtain(m => m.IdUsuario == idUsuario && m.IdGimnasio == idGimnasio);
                if(miembroExist != null)
                    throw new TaskCanceledException($"El usuario ya esta registrado en este Gimnasio");

                Miembro miembroCreado = await _repository.Create(entidad);

                if (miembroCreado.IdMiembro == 0)
                    throw new TaskCanceledException("No se pudo registrar la membrecia");

                IQueryable<Miembro> query = await _repository.Consult(h => h.IdMiembro == miembroCreado.IdMiembro);
                miembroCreado = query.Include(g => g.IdGimnasioNavigation).Include(u => u.IdUsuarioNavigation).First();

                return miembroCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Miembro> Agregar(int idGimnasio, Usuario entidad)
        {
            try
            {
                Usuario usuarioCreado = await _usuarioService.Crear(entidad);

                Miembro miembroCreado = await Crear(idGimnasio, usuarioCreado.IdUsuario);

                if(miembroCreado.IdMiembro == 0)
                    throw new TaskCanceledException("No se pudo registrar la membrecia");
                
                IQueryable<Miembro> query = await _repository.Consult(u => u.IdMiembro == miembroCreado.IdMiembro);
                miembroCreado = query.Include(g => g.IdGimnasioNavigation).Include(u => u.IdUsuarioNavigation).First();

                return miembroCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idMiembro)
        {
            Miembro miembroEncontrado = await GetById(idMiembro);
            if(miembroEncontrado == null)
                throw new TaskCanceledException("La memebresia no existe");

            try
            {   
                bool response = await _repository.Eliminate(miembroEncontrado);
                if(!response)
                    throw new TaskCanceledException("No se puedo eliminar el membresia");
                
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}