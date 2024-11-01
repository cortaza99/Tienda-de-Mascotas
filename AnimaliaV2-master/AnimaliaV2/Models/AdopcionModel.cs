namespace AnimaliaV2.Models
{
    public class AdopcionModel
    {
        public int idAdopcion { get; set; }
        public string fechaAdopcion { get; set; }
        public int fk_idSolicitudAdopcion { get; set; }
    }
}
