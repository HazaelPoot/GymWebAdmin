namespace GymApi.Application.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadStorage(Stream streamArchivo, string carpetaDestino, string nomArchivo);
        Task<bool> DeleteStorage(string carpetaDestino, string nomArchivo);
    }
}