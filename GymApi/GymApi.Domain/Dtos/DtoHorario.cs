namespace GymApi.Domain.Dtos
{
    public class DtoHorario
    {
        public int IdHorario { get; set; }
        public string DiaSemana { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int? IdGimnasio { get; set; }
        public string NombreGimnasio { get; set; }
    }
}