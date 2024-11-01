namespace AnimaliaV2.Models
{
    public class MunicipioModel
    {
        public int IdMunicipio { get; set; }
        public string nombreMunicipio { get; set; }
        public int fk_departamento { get; set; }
    }
}
