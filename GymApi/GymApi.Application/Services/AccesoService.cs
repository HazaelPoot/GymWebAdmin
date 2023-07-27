using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;
using System.Security.Claims;
using GymApi.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GymApi.Application.Services
{
    public class AccesoService : IAccesoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<Gimnasio> _repository;
        private readonly IUtilityService _utilityService;
        public AccesoService(IGenericRepository<Gimnasio> repository, IUtilityService utilityService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _utilityService = utilityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Gimnasio> Authentication(string correo, string password)
        {
            Gimnasio gym_encontrado = await _repository.Obtain(u => u.Correo.Equals(correo));

            if(gym_encontrado is null)
                throw new TaskCanceledException($"No existe el Correo {correo}");

            var userPassword = _utilityService.DesencryptMD5(gym_encontrado.Passw);
            if(userPassword != password)
                throw new TaskCanceledException("La contraseña es incorrecta");

            return gym_encontrado;
        }

        public async Task<Gimnasio> GetUserAuth(int IdGym)
        {
            IQueryable<Gimnasio> query = await _repository.Consult(u => u.IdGimnasio == IdGym);
            Gimnasio result = query.FirstOrDefault();

            return result;
        }

        public async Task<bool> GenerateClaims(DtoGimnasio user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.NameIdentifier, user.IdGimnasio.ToString()),
                new Claim("Profile", user.UrlImagen),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(50)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return true;
        }
    }
}