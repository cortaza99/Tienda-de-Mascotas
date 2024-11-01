namespace AnimaliaV2.Models
{
    public class RegistroMedicoVeterinarioModel
    {
        public int idRegistroMedicoVeterinario { get; set; }
        public string fechaRegistro { get; set; }
        public string dieta { get; set; }
        public string condicionEspecial { get; set; }
        public int fk_idMascota { get; set; }
    }
}
