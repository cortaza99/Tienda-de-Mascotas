using Org.BouncyCastle.Asn1.Mozilla;

namespace AnimaliaV2.Models
{
    public class GestionarSolicitudViewModel
    {
        public int id_solicitud { get; set; }
        public string nombreMascota { get; set; }
        public int id_mascota { get; set; }  
        public string estado_Solicitud { get; set; }    
        public string Nombre_Completo { get; set; } 
        public string telefono { get; set; }    

    }
}
