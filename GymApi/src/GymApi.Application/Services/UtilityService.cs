using System.Text;
using System.Security.Cryptography;
using GymApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GymApi.Application.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IConfiguration _config;
        public UtilityService(IConfiguration config)
        {
            _config = config;
        }

        public string EncryptMD5(string texto)
        {
            try
            {
                string key = _config.GetSection("MD5:Key").Value;
                byte[] keyArray;
                byte[] encryptArray = UTF8Encoding.UTF8.GetBytes(texto);
                
                using (var hashmd5 = MD5.Create())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }

                using var tdes = TripleDES.Create();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResult = cTransform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);

                texto = Convert.ToBase64String(ArrayResult, 0, ArrayResult.Length);
            }
            catch
            {
                
            }

            return texto;
        }

        public string DesencryptMD5(string texto)
        {
            try
            {
                string key = _config.GetSection("MD5:Key").Value;
                byte[] keyArray;
                byte[] desencryptArray = Convert.FromBase64String(texto);

                using (var md5 = MD5.Create())
                {
                    keyArray = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }

                using var tdes = TripleDES.Create();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(desencryptArray, 0, desencryptArray.Length);

                texto = Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                
            }

            return texto;
        }

        public string NameImage(string fileName)
        {
            string nombre_codigo = Guid.NewGuid().ToString("N")[..15];
            string extension = Path.GetExtension(fileName);
            var nombreFoto = string.Concat(nombre_codigo, extension);


            string[] extensionesPermitidas = { ".png", ".jpg", ".jpeg", ".gif", ".ico" };
            if (!extensionesPermitidas.Contains(extension))
            {
                throw new TaskCanceledException($"Los formatos de imagen permitidos son: {string.Join(", ", extensionesPermitidas)}");
            }

            return nombreFoto;
        }
    }
}