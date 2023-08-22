using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using GymApi.Application.Interfaces;

namespace GymApi.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericSqlRepository<Usuario> _repository;
        private readonly IUtilityService _utilityService;

        public UsuarioService(IGenericSqlRepository<Usuario> repository, IUtilityService utilityService)
        {
            _repository = repository;
            _repository = repository;
            _utilityService = utilityService;
        }

        public async Task<List<Usuario>> Lista()
        {
            try
            {
                IQueryable<Usuario> query = await _repository.Consult();
                return query.ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> GetById(int idUsuario)
        {
            Usuario usuarioExist = await _repository.Obtain(m => m.IdUsuario == idUsuario)
                ?? throw new TaskCanceledException($"No existe un Usuario con el ID {idUsuario}");

            try
            {
                IQueryable<Usuario> query = await _repository.Consult(m => m.IdUsuario == idUsuario);
                Usuario result = query.First();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> Crear(Usuario entidad)
        {
            Usuario usuarioExist = await _repository.Obtain(s => s.Nombre == entidad.Nombre);
            if(usuarioExist != null)
                throw new TaskCanceledException($"Ya existe un usuario con el nombre {entidad.Nombre}");
            
            Usuario correoExist = await _repository.Obtain(s => s.Correo == entidad.Correo);
            if(correoExist != null)
                throw new TaskCanceledException($"Ya existe un usuario usando el correo {entidad.Correo}");

            try
            {
                entidad.Passw = _utilityService.EncryptMD5(entidad.Passw);
                entidad.FechaInscripcion = DateTime.Now.ToString("dd/MM/yyyy");
                Usuario usuarioCreado = await _repository.Create(entidad);

                if(usuarioCreado.IdUsuario is 0)
                    throw new TaskCanceledException("No se pudo crear el Usuario");
                
                IQueryable<Usuario> query = await _repository.Consult(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.First();

                return usuarioCreado;
            }
            catch
            {
                throw;
            }
        }

        public Task<Usuario> Editar(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            Usuario usuarioEncontrado = await _repository.Obtain(s => s.IdUsuario == idUsuario)
                ?? throw new TaskCanceledException("El Usuario no existe");

            try
            {   
                bool response = await _repository.Eliminate(usuarioEncontrado);
                if(!response)
                    throw new TaskCanceledException("No se puedo eliminar el Usuario");
                
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}