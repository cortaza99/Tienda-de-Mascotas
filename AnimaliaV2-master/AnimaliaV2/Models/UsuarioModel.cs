namespace AnimaliaV2.Models
{
    public class UsuarioModel
    {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string contraseniaUsuario { get; set; }
        public int fk_idPersona { get; set; }
        public int fk_idRol { get; set; }
    }
}
