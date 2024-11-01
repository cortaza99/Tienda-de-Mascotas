using Newtonsoft.Json;

namespace AnimaliaV2.Models
{
    public class RegistroMedicoMascotaViewModel
    {
        public MascotaModel Mascota { get; set; }
        public RegistroMedicoVeterinarioModel registro { get; set; }
        public List<VacunaModel> vacunas { get; set; }
        public string Antecedentes { get; set; }
        public string VacunasAnimal { get; set; }

    }
}
