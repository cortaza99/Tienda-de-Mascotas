namespace AnimaliaV2.Models
{
    public class SolicitudAdopcionModel
    {
        public int idSolicitudAdopcion { get; set; }  
        public int id_Mascota { get; set; } 
        public string fechaSolicitud { get; set; }  
        public int estado_Solicitud { get; set; }   
        public string descripcionSolicitud { get; set; }
        public int fk_idUsuario { get; set; }   
    }
}
