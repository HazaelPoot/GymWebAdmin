using GymApi.Domain.Interfaces;
using GymApi.Application.Services;
using GymApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GymApi.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GymApi.Infraestructure.Ioc
{
    public static class Dependencies
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            //DATABASES
            services.AddDbContext<SqlDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConexion"));
            });

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
            .AddCookie(opt => {
                opt.Cookie.Name = "Gym_Cookie";
                opt.LoginPath = "/Acceso/Login";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(50);
            });

            //INTERFACES & SERVICES
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericSqlRepository<>));
            services.AddScoped<ISuscripcionService, SuscripcionService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IServicioService, ServicioService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<IHorarioService, HorarioService>();
            services.AddScoped<IMiembroService, MiembroService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAccesoService, AccesoService>();
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IGymService, GymService>();
        }
    }
}