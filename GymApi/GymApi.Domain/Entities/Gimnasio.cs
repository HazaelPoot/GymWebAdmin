namespace GymApi.Domain.Entities
{
    public partial class Gimnasio
    {
        public Gimnasio()
        {
            Horarios = new HashSet<Horario>();
            Miembros = new HashSet<Miembro>();
            Servicios = new HashSet<Servicio>();
        }

        public int IdGimnasio { get; set; }
        public string Nombre { get; set; }
        public string Contacto { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Correo { get; set; }
        public string Passw { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImagen { get; set; }

        public virtual ICollection<Horario> Horarios { get; set; }
        public virtual ICollection<Miembro> Miembros { get; set; }
        public virtual ICollection<Servicio> Servicios { get; set; }
    }
}