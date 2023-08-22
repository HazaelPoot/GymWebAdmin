using System.Net;
using System.Text;
using System.Net.Mail;
using GymApi.Domain.Entities;
using GymApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GymApi.Application.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _config;
        public CorreoService(IUtilityService utilityService, IConfiguration config)
        {
            _config = config;
            _utilityService = utilityService;
        }
        private SmtpClient UseSmtpClient()
        {
            var client = new SmtpClient
            {
                Host = _config["Email:Host"],
                Port = Convert.ToInt32(_config["Email:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential
                {
                    UserName = _config["Email:UserName"],
                    Password = _config["Email:PassWord"]
                }
            };

            return client;
        }

        public async Task RegisterGymEmail(Gimnasio entity)
        {
            try
            {
                string template = File.ReadAllText("Templates/RegisterGym.html");
                string emailContent = new StringBuilder(template)
                    .Replace("{{ToGymName}}", entity.Nombre)
                    .Replace("{{ToCorreo}}", entity.Correo)
                    .Replace("{{ToPassword}}", _utilityService.DesencryptMD5(entity.Passw))
                    .ToString();

                MailMessage message = new()
                {
                    Body = emailContent,
                    IsBodyHtml = true,
                    Subject = "Confirmación de Registro"
                };
                message.To.Add(new MailAddress(entity.Correo));
                message.From = new MailAddress(entity.Correo, "GymWeb Admin");

                using var client = UseSmtpClient();

                await client.SendMailAsync(message);
            }
            catch
            {
                throw;
            }
        }

        public async Task SuscriptionEmail(Suscripcion entity)
        {
            string GymCorreo = entity.IdServicioNavigation.IdGimnasioNavigation.Correo;
            string UserCorreo = entity.IdMiembroNavigation.IdUsuarioNavigation.Correo;
            string GymName = entity.IdServicioNavigation.IdGimnasioNavigation.Nombre;
            string Username = entity.IdMiembroNavigation.IdUsuarioNavigation.Nombre;
            string Costo = entity.IdServicioNavigation.Costo.ToString();
            string Service = entity.IdServicioNavigation.Nombre;
            string ClaveAcceso = entity.ClaveAcceso;
            string FechaPago = entity.FechaPago;

            try
            {
                string template = File.ReadAllText("Templates/SuscriptionService.html");
                string emailContent = new StringBuilder(template)
                    .Replace("{{ToName}}", Username)
                    .Replace("{{ToGimnasio}}", GymName)
                    .Replace("{{ToService}}", Service)
                    .Replace("{{ToCosto}}", Costo)
                    .Replace("{{ToPayDate}}", FechaPago)
                    .Replace("{{ToClave}}", ClaveAcceso)
                    .ToString();

                MailMessage message = new()
                {
                    Body = emailContent,
                    IsBodyHtml = true,
                    Subject = "Confimación de Suscripción",
                    From = new MailAddress(GymCorreo, GymName)
                };
                message.To.Add(new MailAddress(UserCorreo));

                using var client = UseSmtpClient();

                await client.SendMailAsync(message);
            }
            catch
            {
                throw;
            }
        }

        public Task NewpayEmail(Suscripcion entity)
        {
            throw new NotImplementedException();
        }
    }
}