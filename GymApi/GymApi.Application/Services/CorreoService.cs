using System.Net;
using System.Text;
using System.Net.Mail;
using GymApi.Domain.Entities;
using GymApi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GymApi.Application.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _config;
        public CorreoService(IConfiguration config, IUtilityService utilityService)
        {
            _config = config;
            _utilityService = utilityService;
        }

        public void RegisterGymEmail(Gimnasio entity)
        {
            try
            {
                string template = GetEmailRegisterTemplate();
                string emailContent = new StringBuilder(template)
                    .Replace("{{FromAddress}}", "GYMWEB ADMIN")
                    .Replace("{{ToGymName}}", entity.Nombre)
                    .Replace("{{ToCorreo}}", entity.Correo)
                    .Replace("{{ToPassword}}", _utilityService.DesencryptMD5(entity.Passw))
                    .ToString();

                MailMessage message = new MailMessage();
                message.Body = emailContent;
                message.IsBodyHtml = true;
                message.From = new MailAddress("gymwebadmin@correo.com");
                message.To.Add(new MailAddress(entity.Correo));

                using var client = new SmtpClient
                (
                    _config.GetSection("Email:Host").Value,
                    Convert.ToInt32(_config.GetSection("Email:Port").Value)
                );

                client.EnableSsl = true;
                client.Credentials = new NetworkCredential
                (
                    _config.GetSection("Email:UserName").Value,
                    _config.GetSection("Email:PassWord").Value
                );

                client.Send(message);
            }
            catch
            {
                throw;
            }
        }

        public void SuscriptionEmail(Suscripcion entity)
        {
            try
            {
                string template = GetEmailSuscriptionTemplate();
                string emailContent = new StringBuilder(template)
                    .Replace("{{FromAddress}}", entity.IdServicioNavigation.IdGimnasioNavigation.Correo)
                    .Replace("{{ToName}}", entity.IdMiembroNavigation.IdUsuarioNavigation.Nombre)
                    .Replace("{{ToGimnasio}}", entity.IdServicioNavigation.IdGimnasioNavigation.Nombre)
                    .Replace("{{ToService}}", entity.IdServicioNavigation.Nombre)
                    .Replace("{{ToCosto}}", entity.IdServicioNavigation.Costo.ToString())
                    .Replace("{{ToPayDate}}", entity.FechaPago)
                    .Replace("{{ToClave}}", entity.ClaveAcceso)
                    .ToString();

                MailMessage message = new MailMessage();
                message.Body = emailContent;
                message.IsBodyHtml = true;
                message.From = new MailAddress(entity.IdServicioNavigation.IdGimnasioNavigation.Correo);
                message.To.Add(new MailAddress(entity.IdMiembroNavigation.IdUsuarioNavigation.Correo));

                using var client = new SmtpClient
                (
                    _config.GetSection("Email:Host").Value,
                    Convert.ToInt32(_config.GetSection("Email:Port").Value)
                );

                client.EnableSsl = true;
                client.Credentials = new NetworkCredential
                (
                    _config.GetSection("Email:UserName").Value,
                    _config.GetSection("Email:PassWord").Value
                );

                client.Send(message);
            }
            catch
            {
                throw;
            }
        }

        public void NewpayEmail(Suscripcion entity)
        {
            throw new NotImplementedException();
        }

        public string GetEmailRegisterTemplate()
        {
            string template = @"
            <!DOCTYPE html>
            <html lang='es'>

            <head>
                <meta charset='UTF-8'>
                <title>Registro de Gimnasio</title>
            </head>

            <body>
                <table style='width:50%¿'>
                    <tr>
                        <td align='center' colspan='2'>
                            <h2 style='color:#004DAF'>GymAdmin Web</h2>
                        </td>
                    </tr>
                    <tr>
                        <td align='left' colspan='2'>
                            <h4>{{ToGymName}}, gracias por registarte en nuestro sistema de gestión de Gimnasios.</h4>
                            <h4>Sus credenciales para acceder son las siguientes:</h4>
                        </td>
                    </tr>
                    <tr>
                        <td style='width:50px'>
                            <h4 style='color:#004DAF;margin:2px'>Correo:</h4>
                        </td>
                        <td>{{ToCorreo}}</td>
                    </tr>
                    <tr>
                        <td>
                            <h4 style='color:#004DAF;margin:2px'>Contraseña:</h4>
                        </td>
                        <td>{{ToPassword}}</td>
                    </tr>
                </table>
                <br>
                    <p>Por favor no comparta este correo con nadie.</p>
                <br>
                <a style='border:3px solid #004DAF; border-radius:22px; padding: 5px; text-decoration: none; color: #004DAF; font-weight: bold;'
                    href='http://localhost:4200/Login'>Iniciar Sesión
                </a>
                <br>
            </body>

            </html>
            ";

            return template;
        }

        public string GetEmailSuscriptionTemplate()
        {
            string template = @"
            <!DOCTYPE html>
            <html lang='es'>

            <head>
                <meta charset='UTF-8'>
                <title>Registro de Gimnasio</title>
            </head>

            <body>
                <table style='width:100%¿'>
                    <tr>
                        <td align='center' colspan='2'>
                            <h2 style='color:#004DAF'>GymAdmin Web</h2>
                        </td>
                    </tr>
                    <tr>
                        <td align='left' colspan='2'>
                            <h4>{{ToName}}, gracias por suscribirte al servicio {{ToService}} en el gimanasio {{ToGimnasio}}</h4>
                            <h4>Los datos de tu suscripcion son las siguientes:</h4>
                        </td>
                    </tr>
                    <tr>
                        <td style='width:150px'>
                            <h4 style='color:#004DAF;margin:2px'>Servicio:</h4>
                        </td>
                        <td>{{ToService}}</td>
                    </tr>
                    <tr>
                        <td>
                            <h4 style='color:#004DAF;margin:2px'>Costo:</h4>
                        </td>
                        <td>${{ToCosto}}/Mes</td>
                    </tr>
                    <tr>
                        <td>
                            <h4 style='color:#004DAF;margin:2px'>Fecha de pago:</h4>
                        </td>
                        <td>{{ToPayDate}}</td>
                    </tr>
                    <tr>
                        <td>
                            <h4 style='color:#004DAF;margin:2px'>Clave de acceso:</h4>
                        </td>
                        <td>{{ToClave}}</td>
                    </tr>
                </table>
                <br>
                <p>Por favor no comparta este correo con nadie.</p>
                <br>
            </body>

            </html>
            ";

            return template;
        }
    }
}