namespace AnimaliaV2.Models
{
    public class AntecedenteMedicoModel
    {
        public int idAntecedenteMedico { get; set; }
        public string fechaAntecedente { get; set; }
        public string descripcionAntecedente { get; set; }
        public int fk_idRegistroMedicoVeterinario { get; }
    }
}
