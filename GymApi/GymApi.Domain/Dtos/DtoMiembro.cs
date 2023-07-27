namespace GymApi.Domain.Dtos
{
    public class DtoMiembro
    {
        public int IdMiembro { get; set; }
        public int? IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contacto { get; set; }
        public string FechaInscripcion { get; set; }
        public int? IdGimnasio { get; set; }
        public string NombreGimnasio { get; set; }
    }
}