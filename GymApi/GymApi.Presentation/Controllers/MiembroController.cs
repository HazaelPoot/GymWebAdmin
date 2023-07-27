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
    public class MiembroController : ControllerBase
    {
        private readonly IMiembroService _miembroService;
        private readonly IMapper _mapper;
        
        public MiembroController(IMapper mapper, IMiembroService miembroService)
        {
            _mapper = mapper;
            _miembroService = miembroService;
        }

        [HttpGet]
        [Route("List/{id}")]
        public async Task<IActionResult> Lista(int id)
        {
            GenericResponse<DtoMiembro> gResponse = new GenericResponse<DtoMiembro>();

            try
            {
                List<DtoMiembro> dtoMiembro = _mapper.Map<List<DtoMiembro>>(await _miembroService.Lista(id));

                gResponse.Status = true;
                gResponse.Message = "Lista de Miembros";
                gResponse.ListObject = dtoMiembro;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPost]
        [Route("Agregar/{id}")]
        public async Task<IActionResult> Agregar(int id, [FromBody] DtoUsuario dtoUsuario)
        {
            GenericResponse<DtoMiembro> gResponse = new GenericResponse<DtoMiembro>();

            try
            {
                Miembro miembroCreado = await _miembroService.Agregar(id, _mapper.Map<Usuario>(dtoUsuario));
                DtoMiembro dtoMiembro = _mapper.Map<DtoMiembro>(miembroCreado);

                gResponse.Status = true;
                gResponse.Message = "El Usuario se ha agregado al Gimnasio correctamente";
                gResponse.Object = dtoMiembro;

            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        //PENDIENTE QUE NO SEA GET.
        [HttpGet]
        [Route("Membresia/{idG}/{idU}")]
        public async Task<IActionResult> Membresia(int idG, int idU)
        {
            GenericResponse<DtoMiembro> gResponse = new GenericResponse<DtoMiembro>();

            try
            {
                Miembro miembroCreado = await _miembroService.Crear(idG, idU);
                DtoMiembro dtoMiembro = _mapper.Map<DtoMiembro>(miembroCreado);

                gResponse.Status = true;
                gResponse.Message = "El Usuario se ha agregado al Gimnasio correctamente";
                gResponse.Object = dtoMiembro;

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
                gResponse.Status = await _miembroService.Eliminar(id);
                gResponse.Message = "La membresia fue eliminada correctamente";
            }
            catch (Exception ex)
            {
                gResponse.Message = ex.Message;
                if(gResponse.Message ==  "An error occurred while saving the entity changes. See the inner exception for details.")
                    gResponse.Message = "No puedes eliminar este miembro porque actualmente tiene una suscripcion.";

                gResponse.Status = false;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}