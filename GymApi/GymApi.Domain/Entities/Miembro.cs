namespace GymApi.Domain.Entities
{
    public partial class Miembro
    {
        public Miembro()
        {
            Suscripcions = new HashSet<Suscripcion>();
        }

        public int IdMiembro { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdGimnasio { get; set; }

        public virtual Gimnasio IdGimnasioNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Suscripcion> Suscripcions { get; set; }
    }
}