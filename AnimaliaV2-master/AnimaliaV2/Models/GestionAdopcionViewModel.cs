namespace AnimaliaV2.Models
{
    public class GestionAdopcionViewModel
    {
        public int id_Solicitud { get; set; } 
        public int idMascota { get; set; }  
        public  string documentoPersona { get; set; }
        public string Nombre_Completo { get; set;}
        public string nombreMascota { get; set;}
        public string fechaSolicitud { get;set; }

    }
}
