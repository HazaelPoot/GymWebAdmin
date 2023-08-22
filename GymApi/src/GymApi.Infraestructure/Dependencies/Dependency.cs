using GymApi.Domain.Interfaces;
using GymApi.Application.Mappings;
using GymApi.Application.Services;
using GymApi.Infraestructure.Data;
using GymApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GymApi.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GymApi.Infraestructure.Dependencies
{
    public static class Dependency
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            //DATABASES
            services.AddDbContext<SqlDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConexion"));
            });

            //AUTOMAPPER
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            //ENABLE CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("GymWebPolicy", app =>
                {
                    app.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            //AUTHENTICATION CLAIMS
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.Cookie.Name = "Gym_Cookie";
                opt.LoginPath = "/Acceso/Login";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(50);
            });

            //INTERFACES & SERVICES
            services.AddTransient(typeof(IGenericSqlRepository<>), typeof(GenericSqlRepository<>));
            services.AddScoped<ISuscripcionService, SuscripcionService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IServicioService, ServicioService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<IHorarioService, HorarioService>();
            services.AddScoped<IMiembroService, MiembroService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccesoService, AccesoService>();
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IGymService, GymService>();

        }
    }
}