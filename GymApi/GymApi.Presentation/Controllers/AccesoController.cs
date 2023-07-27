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
            GenericResponse<DtoGimnasio> gResponse = new GenericResponse<DtoGimnasio>();
            
            try
            {
                Gimnasio auth = await _accesoService.Authentication(dto.Correo, dto.Passw);
                DtoGimnasio dtoGimnasio = _mapper.Map<DtoGimnasio>(await _accesoService.GetUserAuth(auth.IdGimnasio));
                await _accesoService.GenerateClaims(dtoGimnasio);
                
                gResponse.Status = true;
                gResponse.Message = "Exito";
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