using AutoMapper;
using Newtonsoft.Json;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Dtos.Response;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;

        public ServicioController(IServicioService servicioService, IUtilityService utilityService, IMapper mapper)
        {
            _mapper = mapper;
            _utilityService = utilityService;
            _servicioService = servicioService;
        }

        [HttpGet]
        [Route("List/{id}")]
        public async Task<IActionResult> Lista(int id)
        {
            GenericResponse<DtoServicio> gResponse = new GenericResponse<DtoServicio>();

            try
            {
                List<DtoServicio> dtoServicio = _mapper.Map<List<DtoServicio>>(await _servicioService.Lista(id));

                gResponse.Status = true;
                gResponse.Message = "Lista de servicios";
                gResponse.ListObject = dtoServicio;
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
            GenericResponse<DtoServicio> gResponse = new GenericResponse<DtoServicio>();

            try
            {
                DtoServicio dtoServicio = _mapper.Map<DtoServicio>(await _servicioService.GetById(id));

                gResponse.Status = true;
                gResponse.Message = "Servicio Encontrado";
                gResponse.Object = dtoServicio;
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
        public async Task<IActionResult> Crear(int id, [FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoServicio> gResponse = new GenericResponse<DtoServicio>();

            try
            {
                DtoServicio dtoServicio = JsonConvert.DeserializeObject<DtoServicio>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    nombreFoto = _utilityService.NameImage(foto.FileName);
                    fotoStream = foto.OpenReadStream();
                }

                Servicio servicioCreado = await _servicioService.Crear(id, _mapper.Map<Servicio>(dtoServicio), fotoStream, "Gym_Servicios", nombreFoto);
                dtoServicio = _mapper.Map<DtoServicio>(servicioCreado);

                gResponse.Status = true;
                gResponse.Message = "El servicio se ha creado correctamente";
                gResponse.Object = dtoServicio;

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
        public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoServicio> gResponse = new GenericResponse<DtoServicio>();

            try
            {
                DtoServicio dtoServicio = JsonConvert.DeserializeObject<DtoServicio>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    nombreFoto = dtoServicio.NombreFoto;
                    fotoStream = foto.OpenReadStream();
                }

                Servicio servicioEditado = await _servicioService.Editar(_mapper.Map<Servicio>(dtoServicio), fotoStream, "Gym_Servicios", nombreFoto);
                dtoServicio = _mapper.Map<DtoServicio>(servicioEditado);

                gResponse.Status = true;
                gResponse.Message = "La información del servicio se ha actualizada correctamente";
                gResponse.Object = dtoServicio;

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
                gResponse.Status = await _servicioService.Eliminar(id);
                gResponse.Message = "La información del servicio se ha eliminada correctamente";
            }
            catch (Exception ex)
            {
                gResponse.Message = ex.Message;
                if(gResponse.Message ==  "An error occurred while saving the entity changes. See the inner exception for details.")
                    gResponse.Message = "No puedes eliminar este servicio porque hay miembros suscritos a el.";

                gResponse.Status = false;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}