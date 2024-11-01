namespace AnimaliaV2.Models
{
    public class PermisosMenuModel
    {
        public int idPermisos_rol { get; set; }
        public int id_usuario { get; set; }
        public int fk_idRol { get; set; }
        public int idPermisos { get; set; }
        public int nombre_permiso { get; set; }
        public int padre { get; set; }
    }
}
