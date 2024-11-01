using Microsoft.AspNetCore.Mvc;

namespace AnimaliaV2.Models
{
    public class MascotaModel
    {
        public int idMascota { get; set; }
        [BindProperty]
        public string descripcionMascota { get; set; }
        public int alturaMascota { get; set; }
        public float pesoMascota { get; set; }
        public string colorMascota { get; set; }
        public string nacimientoMascota { get; set; }
        public string estadoAdopcionMascota { get; set; }
        public string nombreMascota { get; set; }
        public string mascota_rescatada { get; set; }
        public string fechaRegistro { get; set; }
        public int publicar { get; set; }
        public string nombreImagen { get; set; }
        public int fk_idRaza { get; set; }
        [BindProperty]
        public int fk_idSexoAnimal { get; set; }
        public int fk_idUsuario { get; set; }
        public int fk_idMunicipio { get; set; }
        public string imgUbicacionExterna { get;set; }
    }
}
