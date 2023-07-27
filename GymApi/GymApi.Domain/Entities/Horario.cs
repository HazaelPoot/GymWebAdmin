namespace GymApi.Domain.Entities
{
    public partial class Horario
    {
        public int IdHorario { get; set; }
        public string DiaSemana { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int? IdGimnasio { get; set; }

        public virtual Gimnasio IdGimnasioNavigation { get; set; }
    }
}