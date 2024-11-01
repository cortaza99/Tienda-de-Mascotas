namespace AnimaliaV2.Models
{
    public class RazaMascotaModel
    {
        public int idRazaMascota { get; set; }
        public string razaMascota { get; set; }
        public int fk_idEspecieMascota { get; set; }
    }
}
