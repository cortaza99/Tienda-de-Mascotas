using AnimaliaV2.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace AnimaliaV2.Data
{
    public class InvitadoManejador
    {
        public MySqlConnection _connection;

        public InvitadoManejador(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<MascotaModel> listarMascotas()
        {
            List<MascotaModel> list = new List<MascotaModel>();

            try {
                using(_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ver_mascotas", _connection)) {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new MascotaModel
                                {
                                    idMascota = Convert.ToInt32(reader["idMascota"]),
                                    nombreMascota = reader["nombreMascota"].ToString(),
                                    descripcionMascota = reader["descripcionMascota"].ToString(),
                                    imgUbicacionExterna = reader["imgUbicacion"].ToString()
                                });
                            }
                        }
                    }
                    _connection.Close();
                }
                //Traer imagen de la carpeta wwwroot para cada modelo
                //foreach (MascotaModel model in list) {
                //    FileStream stream = new FileStream(model.imgUbicacionExterna, FileMode.Open);
                //    IFormFile imagen = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(model.imgUbicacionExterna));
                //    model.imgMascota= imagen;
                //}
                return list;
            }catch (Exception ex)
            {
                return list;
            }
        }


        public bool GuardarPersona(RegistroPersonaViewModel registropersona)
        {
            PersonaModel persona = registropersona.Persona;
            UsuarioModel usuario = registropersona.Usuario;
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("insertar_persona", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@doc", persona.documentoPersona);
                        command.Parameters.AddWithValue("@pri_nombre", persona.nombrePersona);
                        command.Parameters.AddWithValue("@seg_nombre", persona.snombrePersona);
                        command.Parameters.AddWithValue("@pri_apellido",persona.apellidoPersona);
                        command.Parameters.AddWithValue("@seg_apellido",persona.sapellidoPersona);
                        command.Parameters.AddWithValue("@fecha",persona.fechaNacPersona);
                        command.Parameters.AddWithValue("@direccion_", persona.direcccion);
                        command.Parameters.AddWithValue("@correo", persona.correo_electronico);
                        command.Parameters.AddWithValue("@celu", persona.telefono);
                        command.Parameters.AddWithValue("@nombre", usuario.nombreUsuario);
                        command.Parameters.AddWithValue("@contrasenia", usuario.contraseniaUsuario);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                flag = false;
            }
            return flag;
        }


        public List<UsuarioModel> listarUsuarios()
        {
            List<UsuarioModel> list = new List<UsuarioModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM listado_usuarios", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new UsuarioModel
                                {
                                    idUsuario = Convert.ToInt32(reader["idusuario"]),
                                    nombreUsuario = reader["nombreUsuario"].ToString(),
                                    contraseniaUsuario = reader["contraseniaUsuario"].ToString(),
                                    fk_idRol = reader["nombrerol"].ToString() == "Cliente" ? 2 : 1
                                });
                            }
                        }
                    }
                    _connection.Close();
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }


        public bool validarUsuario(string usuario)
        {
            UsuarioModel model = listarUsuarios().Where(item => item.nombreUsuario == usuario).FirstOrDefault();
            if (model == null)
            {
                return false;
            }
            else {
                return true;
            }
        }



    }
}
