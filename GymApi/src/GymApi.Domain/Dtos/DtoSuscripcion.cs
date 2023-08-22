namespace GymApi.Domain.Dtos
{
    public class DtoSuscripcion
    {
        public int IdInscripcion { get; set; }
        public int? IdMiembro { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? IdServicio { get; set; }
        public string NombreServicio { get; set; }
        public string ClaveAcceso { get; set; }
        public string FechaPago { get; set; }
        public double MontoPagar { get; set; }
        public int Pago { get; set; }
        public string ClasePago { get; set; }
    }
}