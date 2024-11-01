namespace AnimaliaV2.Models
{
    public class VacunaAnimalModel
    {
        public int idVacunaAnimal { get; set; }
        public string fechaVacunaAnimal { get; set; }
        public int fk_idRegistroMedicoVeterinario { get; set; }
        public int fk_idVacuna { get; set; }
    }
}
