using AutoMapper;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Dtos.Response;
using GymApi.Application.Interfaces;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionController : ControllerBase
    {
        private readonly ISuscripcionService _suscrService;
        private readonly IMapper _mapper;

        public SuscripcionController(IMapper mapper, ISuscripcionService suscrService)
        {
            _mapper = mapper;
            _suscrService = suscrService;
        }

        [HttpGet]
        [Route("List/{id}")]
        public async Task<IActionResult> List(int id)
        {
            GenericResponse<DtoSuscripcion> gResponse = new();

            try
            {
                List<DtoSuscripcion> dtoSuscripcion = _mapper.Map<List<DtoSuscripcion>>(await _suscrService.Lista(id));

                gResponse.Status = true;
                gResponse.Message = "Lista de Suscripciones";
                gResponse.ListObject = dtoSuscripcion;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpGet]
        [Route("Search/{id}/{value}")]
        public async Task<IActionResult> Search(int id, string value)
        {
            GenericResponse<DtoSuscripcion> gResponse = new();

            try
            {
                DtoSuscripcion dtoSuscripcion = _mapper.Map<DtoSuscripcion>(await _suscrService.Validar(id, value));

                gResponse.Status = true;
                gResponse.Message = $"{dtoSuscripcion.Nombre} {dtoSuscripcion.Apellido} puede acceder al Gimnasio";
                gResponse.Object = dtoSuscripcion;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] DtoSuscripcion dto)
        {
            GenericResponse<DtoSuscripcion> gResponse = new();

            try
            {
                DtoSuscripcion dtoSuscripcion = _mapper.Map<DtoSuscripcion>(await _suscrService.Crear(_mapper.Map<Suscripcion>(dto)));

                gResponse.Status = true;
                gResponse.Message = "El Usuario fue suscrito al servicio correctamente";
                gResponse.Object = dtoSuscripcion;

            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPut]
        [Route("ChangePay")]
        public async Task<IActionResult> ChangePay([FromBody] DtoSuscripcion dto)
        {
            GenericResponse<DtoSuscripcion> gResponse = new();

            try
            {
                DtoSuscripcion dtoSuscripcion = _mapper.Map<DtoSuscripcion>(await _suscrService.CambiarPago(_mapper.Map<Suscripcion>(dto)));

                gResponse.Status = true;
                gResponse.Message = "Se aplicío el pago correspondiente";
                gResponse.Object = dtoSuscripcion;

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
            GenericResponse<bool> gResponse = new();

            try
            {
                gResponse.Status = await _suscrService.Eliminar(id);
                gResponse.Message = "La suscripción fue eliminada correctamente";
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