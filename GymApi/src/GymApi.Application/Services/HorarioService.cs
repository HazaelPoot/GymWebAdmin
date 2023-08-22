using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GymApi.Application.Interfaces;

namespace GymApi.Application.Services
{
    public class HorarioService : IHorarioService
    {
        private readonly IGenericSqlRepository<Horario> _repository;
        private readonly IGymService _gymService;

        public HorarioService(IGenericSqlRepository<Horario> repository, IGymService gymService)
        {
            _gymService = gymService;
            _repository = repository;
        }

        public async Task<List<Horario>> Lista(int idGimnasio)
        {
            Gimnasio gymExist = await _gymService.ObtenerGym(idGimnasio)
                ?? throw new TaskCanceledException($"No existe un Gimnasio con el ID {idGimnasio}");
            
            try
            {
                IQueryable<Horario> query = await _repository.Consult(h => h.IdGimnasio == idGimnasio);
                return query.Include(g => g.IdGimnasioNavigation).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Horario> GetById(int idHorario)
        {
            Horario horarioExist = await _repository.Obtain(h => h.IdHorario == idHorario)
                ?? throw new TaskCanceledException($"No existe un Horario con el ID {idHorario}");

            try
            {
                IQueryable<Horario> query = await _repository.Consult(h => h.IdHorario == idHorario);
                Horario result = query.Include(g => g.IdGimnasioNavigation).First();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Horario> Crear(int idGimnasio, Horario entidad)
        {
            IQueryable<Horario> horarioEnGimnasio = await _repository.Consult(s => s.IdGimnasio == idGimnasio && s.DiaSemana == entidad.DiaSemana);
            if(horarioEnGimnasio.Any())
                throw new TaskCanceledException($"Ya existe un horario para el día {entidad.DiaSemana} en este Gimnasio");

            try
            {
                entidad.IdGimnasio = idGimnasio;
                Horario horarioCreado = await _repository.Create(entidad);

                if(horarioCreado.IdGimnasio == 0)
                    throw new TaskCanceledException("No se pudo crear el horario");
                
                IQueryable<Horario> query = await _repository.Consult(h => h.IdHorario == horarioCreado.IdHorario);
                horarioCreado = query.Include(g => g.IdGimnasioNavigation).First();

                return horarioCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Horario> Editar(Horario entidad)
        {
            IQueryable<Horario> horarioEnGimnasio = await _repository.Consult(s => s.IdGimnasio == entidad.IdGimnasio && s.DiaSemana == entidad.DiaSemana && s.IdHorario != entidad.IdHorario);
            if(horarioEnGimnasio.Any())
                throw new TaskCanceledException($"Ya existe un horario para el día {entidad.DiaSemana} en este Gimnasio");
            
            try
            {
                IQueryable<Horario> queryHorario = await _repository.Consult(h => h.IdHorario == entidad.IdHorario);
                Horario horario_editar = queryHorario.First();
                horario_editar.DiaSemana = entidad.DiaSemana;
                horario_editar.HoraInicio = entidad.HoraInicio;
                horario_editar.HoraFin = entidad.HoraFin;

                bool response = await _repository.Edit(horario_editar);
                if(!response)
                    throw new TaskCanceledException("No se pudo moficiar el horario");
                
                Horario horario_editado = queryHorario.Include(h => h.IdGimnasioNavigation).First();

                return horario_editado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idHorario)
        {
            Horario horarioEncontrado = await _repository.Obtain(s => s.IdHorario == idHorario)
                ?? throw new TaskCanceledException("El horario no existe");

            try
            {   
                bool response = await _repository.Eliminate(horarioEncontrado);
                if(!response)
                    throw new TaskCanceledException("No se puedo eliminar el horario");
                
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EliminarTodo(int idGimnasio)
        {
            try
            {
                List<Horario> horarios = await Lista(idGimnasio);
                bool response = await _repository.EliminateRange<Horario>(h => h.IdGimnasio == idGimnasio);

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}