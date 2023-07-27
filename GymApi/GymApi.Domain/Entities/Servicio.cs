namespace GymApi.Domain.Entities
{
    public partial class Servicio
    {
        public Servicio()
        {
            Suscripcions = new HashSet<Suscripcion>();
        }

        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public string Detalles { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImagen { get; set; }
        public int? IdGimnasio { get; set; }

        public virtual Gimnasio IdGimnasioNavigation { get; set; }
        public virtual ICollection<Suscripcion> Suscripcions { get; set; }
    }
}