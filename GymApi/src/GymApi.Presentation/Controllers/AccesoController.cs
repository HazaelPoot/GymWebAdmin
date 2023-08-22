using AutoMapper;
using GymApi.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using GymApi.Domain.Dtos.Response;
using GymApi.Application.Interfaces;

namespace GymApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoController : ControllerBase
    {
        private readonly IAccesoService _accesoService;
        private readonly IMapper _mapper;

        public AccesoController(IAccesoService accesoService, IMapper mapper)
        {
            _mapper = mapper;
            _accesoService = accesoService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] DtoLogin dto)
        {
            GenericResponse<DtoGimnasio> gResponse = new();
            
            try
            {
                var auth = await _accesoService.Authentication(dto.Correo, dto.Passw);
                DtoGimnasio dtoGimnasio = _mapper.Map<DtoGimnasio>(await _accesoService.GetGymAuth(auth.IdGimnasio));
                
                gResponse.Status = true;
                gResponse.Message = "Acceso permitido";
                gResponse.Object = dtoGimnasio;
            }
            catch(Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}