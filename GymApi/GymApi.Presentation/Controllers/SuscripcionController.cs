using AutoMapper;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Dtos.Response;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISuscripcionService _suscrService;

        public SuscripcionController(ISuscripcionService suscrService, IMapper mapper)
        {
            _suscrService = suscrService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("List/{id}")]
        public async Task<IActionResult> List(int id)
        {
            GenericResponse<DtoSuscripcion> gResponse = new GenericResponse<DtoSuscripcion>();

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
            GenericResponse<DtoSuscripcion> gResponse = new GenericResponse<DtoSuscripcion>();

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
            GenericResponse<DtoSuscripcion> gResponse = new GenericResponse<DtoSuscripcion>();

            try
            {
                Suscripcion susCreado = await _suscrService.Crear(_mapper.Map<Suscripcion>(dto));
                DtoSuscripcion dtoSuscripcion = _mapper.Map<DtoSuscripcion>(susCreado);

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
        public async Task<IActionResult> ChangePay([FromBody] DtoSuscripcion dtoSuscripcion)
        {
            GenericResponse<DtoSuscripcion> gResponse = new GenericResponse<DtoSuscripcion>();

            try
            {
                Suscripcion suscrpEditado = await _suscrService.CambiarPago(_mapper.Map<Suscripcion>(dtoSuscripcion));
                dtoSuscripcion = _mapper.Map<DtoSuscripcion>(suscrpEditado);

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
            GenericResponse<string> gResponse = new GenericResponse<string>();

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