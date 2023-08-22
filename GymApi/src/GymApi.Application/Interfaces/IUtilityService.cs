namespace GymApi.Application.Interfaces
{
    public interface IUtilityService
    {
        string EncryptMD5(string texto);
        string DesencryptMD5(string texto);
        string NameImage(string fileName);
    }
}