namespace GymApi.Domain.Dtos
{
    public class DtoServicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public string Detalles { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImagen { get; set; }
        public int? IdGimnasio { get; set; }
        public string NombreGimnasio { get; set; }
    }
}