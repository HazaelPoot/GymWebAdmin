namespace GymApi.Domain.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            Miembros = new HashSet<Miembro>();
        }

        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public string Passw { get; set; }
        public string FechaInscripcion { get; set; }

        public virtual ICollection<Miembro> Miembros { get; set; }
    }
}
