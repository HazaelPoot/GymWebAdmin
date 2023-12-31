namespace GymApi.Domain.Dtos.Response
{
    public class GenericResponse <TObject>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public TObject Object { get; set; }
        public List<TObject> ListObject { get; set; }
    }
}