using AutoMapper;
using Newtonsoft.Json;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Dtos.Response;
using GymApi.Application.Interfaces;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IAccountService _accountService;
        private readonly IGymService _gymService;
        private readonly IMapper _mapper;

        public GymController(IMapper mapper, IGymService gymService, IUtilityService utilityService, IAccountService accountService)
        {
            _mapper = mapper;
            _gymService = gymService;
            _accountService = accountService;
            _utilityService = utilityService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Obetner(int id)
        {
            GenericResponse<DtoGimnasio> gResponse = new();

            try
            {
                DtoGimnasio dtoGimnasio = _mapper.Map<DtoGimnasio>(await _gymService.ObtenerGym(id));

                gResponse.Status = true;
                gResponse.Object = dtoGimnasio;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoGimnasio> gResponse = new();

            try
            {
                DtoGimnasio dtoGimnasio = JsonConvert.DeserializeObject<DtoGimnasio>(modelo);
                string nombreLogo = "";
                Stream logoStream = null;

                if(foto != null)
                {
                    nombreLogo = _utilityService.NameImage(foto.FileName);
                    logoStream = foto.OpenReadStream();
                }

                DtoGimnasio gimnasioCreado = _mapper.Map<DtoGimnasio>(await _gymService.Crear(_mapper.Map<Gimnasio>(dtoGimnasio), logoStream, "Logo_Gym", nombreLogo));
                dtoGimnasio = gimnasioCreado;

                gResponse.Status = true;
                gResponse.Message = "Su registro fue exitoso!";
                gResponse.Object = dtoGimnasio;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        [Route("SaveProfile")]
        public async Task<IActionResult> GuardarPerfil([FromForm]IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoGimnasio> gResponse = new();

            try
            {
                DtoGimnasio dtoGimnasio = JsonConvert.DeserializeObject<DtoGimnasio>(modelo);

                string nombreLogo = "";
                Stream logoStream = null;

                if(foto != null)
                {
                    nombreLogo = dtoGimnasio.Nombre;
                    logoStream = foto.OpenReadStream();
                }

                DtoGimnasio gym_editado = _mapper.Map<DtoGimnasio>(await _gymService.GuardarPerfil(_mapper.Map<Gimnasio>(dtoGimnasio), logoStream, "Logo_Gym", nombreLogo)); 
                dtoGimnasio = gym_editado;

                gResponse.Status = true;
                gResponse.Message = "Su información fue actualizada con exito";
                gResponse.Object = dtoGimnasio;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        [Route("ChangePass/{id}")]
        public async Task<IActionResult> CambiarClave(int id, [FromBody] DtoPassword dto)
        {
            GenericResponse<bool> response = new();
            try
            {
                bool result = await _accountService.CambiarClave(id, dto.ClaveActual, dto.ClaveNueva);

                response.Status = true;
                response.Message = "La Contraseña se ha cambiado con exito";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [Route("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id, [FromBody] DtoPassword dto)
        {
            GenericResponse<bool> response = new();
            try
            {
                bool result = await _accountService.EliminarPerfil(id, dto.ClaveActual);

                response.Status = true;
                response.Message = "La cuenta se ha eliminado con exito";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}