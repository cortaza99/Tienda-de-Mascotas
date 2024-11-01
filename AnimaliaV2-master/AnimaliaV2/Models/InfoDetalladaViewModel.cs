using System.Security.Policy;

namespace AnimaliaV2.Models
{
    public class InfoDetalladaViewModel
    {
        public MascotaModel Mascota { get; set; }
        public string dieta { get; set; }
        public string raza { get; set; }
        public string especie { get; set; }
        public string ubicacion { get; set; }
        public string mascotaRescatada { get; set; }
        public string condicionEspecial { get; set; }
        public List<VacunaModel> vacunas { get; set; }
        public List<AntecedenteMedicoModel> antecedentes { get; set; }
    }
}
