using AutoMapper;
using Newtonsoft.Json;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Interfaces;
using GymApi.Domain.Dtos.Response;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _horarioService;
        private readonly IMapper _mapper;
        
        public HorarioController(IHorarioService horarioService, IMapper mapper)
        {
            _mapper = mapper;
            _horarioService = horarioService;
        }

        [HttpGet]
        [Route("List/{id}")]
        public async Task<IActionResult> Lista(int id)
        {
            GenericResponse<DtoHorario> gResponse = new GenericResponse<DtoHorario>();

            try
            {
                List<DtoHorario> dtoHorario = _mapper.Map<List<DtoHorario>>(await _horarioService.Lista(id));

                gResponse.Status = true;
                gResponse.Message = "Lista de Horarios";
                gResponse.ListObject = dtoHorario;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            GenericResponse<DtoHorario> gResponse = new GenericResponse<DtoHorario>();

            try
            {
                DtoHorario dtoHorario = _mapper.Map<DtoHorario>(await _horarioService.GetById(id));

                gResponse.Status = true;
                gResponse.Message = "Horario Encontrado";
                gResponse.Object = dtoHorario;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPost]
        [Route("Create/{id}")]
        public async Task<IActionResult> Crear(int id, [FromForm] string modelo)
        {
            GenericResponse<DtoHorario> gResponse = new GenericResponse<DtoHorario>();

            try
            {
                DtoHorario dtoHorario = JsonConvert.DeserializeObject<DtoHorario>(modelo);

                Horario horarioCreado = await _horarioService.Crear(id, _mapper.Map<Horario>(dtoHorario));
                dtoHorario = _mapper.Map<DtoHorario>(horarioCreado);

                gResponse.Status = true;
                gResponse.Message = "El horario se ha creado correctamente";
                gResponse.Object = dtoHorario;

            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Editar([FromForm] string modelo)
        {
            GenericResponse<DtoHorario> gResponse = new GenericResponse<DtoHorario>();

            try
            {
                DtoHorario dtoHorario = JsonConvert.DeserializeObject<DtoHorario>(modelo);

                Horario horarioEditado = await _horarioService.Editar(_mapper.Map<Horario>(dtoHorario));
                dtoHorario = _mapper.Map<DtoHorario>(horarioEditado);

                gResponse.Status = true;
                gResponse.Message = "La información del horario fue actualizada correctamente";
                gResponse.Object = dtoHorario;

            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }
        
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Status = await _horarioService.Eliminar(id);
                gResponse.Message = "La información del horario fue eliminada correctamente";
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}