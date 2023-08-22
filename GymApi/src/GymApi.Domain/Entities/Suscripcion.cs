namespace GymApi.Domain.Entities
{
    public partial class Suscripcion
    {
        public int IdInscripcion { get; set; }
        public int Pago { get; set; }
        public string FechaPago { get; set; }
        public string ClaveAcceso { get; set; }
        public int IdMiembro { get; set; }
        public int IdServicio { get; set; }
        public string ClasePago { get; set; }

        public virtual Miembro IdMiembroNavigation { get; set; }
        public virtual Servicio IdServicioNavigation { get; set; }
    }
}
