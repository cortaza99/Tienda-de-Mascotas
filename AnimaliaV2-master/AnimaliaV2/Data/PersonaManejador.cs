using AnimaliaV2.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;


namespace AnimaliaV2.Data
{
    public class PersonaManejador
    {
        public MySqlConnection _connection;

        public PersonaManejador(MySqlConnection connection)
        {
            _connection = connection;
        }


        public List<SolicitudAdopcionViewModel> listarmisSolicitudes(int idUsuario)
        {
            List<SolicitudAdopcionViewModel> list = new List<SolicitudAdopcionViewModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("ver_mis_solicitudes", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idUsuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SolicitudAdopcionViewModel
                                {
                                    idSolicitudAdopcion = Convert.ToInt32(reader["idSolicitudAdopcion"]),
                                    estado_Solicitud = reader["Estado"].ToString(),                             
                                    nombreMascota = reader["nombreMascota"].ToString(),                                
                                    especieMascota = reader["especieMascota"].ToString(),
                                    ubicacionMascota = reader["Ubicación"].ToString()
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


        public bool EliminarSolicitud(int id)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminar_solicitud", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id);
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

        public List<GestionarSolicitudViewModel> gestionar_solicitudes(int idUsuario)
        {
            List<GestionarSolicitudViewModel> list = new List<GestionarSolicitudViewModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("gestionar_solicitudes", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idUsuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new GestionarSolicitudViewModel
                                {
                                    id_solicitud = Convert.ToInt32(reader["id_solicitud"]),
                                    nombreMascota  = reader["nombreMascota"].ToString(),
                                    id_mascota= Convert.ToInt32(reader["id_Mascota"]),
                                    estado_Solicitud = reader["Estado"].ToString(),
                                    Nombre_Completo = reader["Nombre Completo"].ToString(),
                                    telefono = reader["telefono"].ToString()
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



        public bool denegarSolicitud(int id)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("denegar_solicitud", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id); 
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


        public bool AprobarSolicitud(int id_solicitud, int id_mascota)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("aprobar_solicitud", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id_solicitud", id_solicitud);
                        command.Parameters.AddWithValue("@id_mascota", id_mascota);
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


        public List<GestionAdopcionViewModel> gestionar_donaciones(int idUsuario)
        {
            List<GestionAdopcionViewModel> list = new List<GestionAdopcionViewModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("gestionar_donaciones", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idUsuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new GestionAdopcionViewModel
                                {
                                    id_Solicitud = Convert.ToInt32(reader["id_solicitud"]),
                                    idMascota = Convert.ToInt32(reader["idMascota"]),
                                    documentoPersona= reader["documentoPersona"].ToString(),
                                    Nombre_Completo = reader["Nombre Completo"].ToString(),
                                    nombreMascota = reader["nombreMascota"].ToString(),
                                    fechaSolicitud = reader["fechaSolicitud"].ToString()
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

        public bool donarMascota(int id_solicitud, int id_mascota)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("donar_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id_solicitud);
                        command.Parameters.AddWithValue("@id_mascota", id_mascota);
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

        public bool eliminar_adopcion(int id_solicitud, int id_mascota)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminar_adopcion", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id_solicitud", id_solicitud);
                        command.Parameters.AddWithValue("@id_mascota", id_mascota);
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


        public List<MisMascotasModel> misMascotas(int idUsuario)
        {
            List<MisMascotasModel> list = new List<MisMascotasModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("ver_mis_mascotas", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idUsuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new MisMascotasModel
                                {
                                    idMascota = Convert.ToInt32(reader["idMascota"]),
                                    nombreMascota = reader["nombreMascota"].ToString(),
                                    especieMascota = reader["especieMascota"].ToString(),
                                    ubicacion = reader["Ubicación"].ToString(),
                                    idRegistroMedico = Convert.ToInt32(reader["idRegistroMedicoVeterinario"])

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

        public bool EliminarMascota(int id_mascota)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminar_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id_mascota", id_mascota);
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


        public PersonaModel ConsultarPersona(int idUsuario)
        {
            PersonaModel list = null;

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("consultar_persona", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idUsuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                list = new PersonaModel
                                {
                                    idPersona = Convert.ToInt32(reader["idPersona"]),
                                    documentoPersona = reader["documentoPersona"].ToString(),
                                    nombrePersona = reader["nombrePersona"].ToString(),
                                    snombrePersona = reader["snombrePersona"].ToString(),
                                    apellidoPersona = reader["apellidoPersona"].ToString(),
                                    sapellidoPersona = reader["sapellidoPersona"].ToString(),
                                    fechaNacPersona = reader["fechaNacPersona"].ToString(),
                                    direcccion = reader["direccion"].ToString(),
                                    correo_electronico = reader["correo_electronico"].ToString(),
                                    telefono = reader["telefono"].ToString(),
                                };
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
        public bool ModificarPersona(PersonaModel persona)
        {
     
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("modificar_persona", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", persona.idPersona);
                        command.Parameters.AddWithValue("@celular", persona.telefono);
                        command.Parameters.AddWithValue("@correo", persona.correo_electronico);
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
        public List<PersonaModel> ListadoPersonas()
        {
            List<PersonaModel> list = new List<PersonaModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM listado_personas", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new PersonaModel
                                {
                                    idPersona = Convert.ToInt32(reader["idPersona"]),
                                    documentoPersona = reader["documentoPersona"].ToString(),
                                    nombrePersona  = reader["Nombre Completo"].ToString(),
                                    fechaNacPersona = reader["fechaNacPersona"].ToString(),
                                    direcccion = reader["direccion"].ToString(),
                                    correo_electronico = reader["correo_electronico"].ToString(),
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

        public bool EditarPersona(PersonaModel persona)
        {

            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("editar_persona", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", persona.idPersona);
                        command.Parameters.AddWithValue("@correo", persona.correo_electronico);
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

        public PersonaModel ConsultarIdPersona(int id_persona)
        {
            PersonaModel list = null;

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("consultar_persona", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id_persona);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                list = new PersonaModel
                                {
                                    idPersona = Convert.ToInt32(reader["idPersona"]),
                                    documentoPersona = reader["documentoPersona"].ToString(),
                                    nombrePersona = reader["nombrePersona"].ToString(),
                                    snombrePersona = reader["snombrePersona"].ToString(),
                                    apellidoPersona = reader["apellidoPersona"].ToString(),
                                    sapellidoPersona = reader["sapellidoPersona"].ToString(),
                                    fechaNacPersona = reader["fechaNacPersona"].ToString(),
                                    direcccion = reader["direccion"].ToString(),
                                    correo_electronico = reader["correo_electronico"].ToString(),
                                    telefono = reader["telefono"].ToString(),
                                };
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
        public List<UsuarioViewModel> ListadoUsuarios()
        {
            List<UsuarioViewModel> list = new List<UsuarioViewModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM Listado_Usuarios", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new UsuarioViewModel
                                {
                                    idUsuario = Convert.ToInt32(reader["idusuario"]),
                                    nombreUsuario = reader["nombreUsuario"].ToString(),
                                    contraseniaUsuario = reader["contraseniaUsuario"].ToString(),
                                    rol = reader["nombrerol"].ToString(),
                                   
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


        public UsuarioViewModel ConsultarUsuario(int id_usuario)
        {
            UsuarioViewModel list = null;

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("consultar_usuario", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id_usuario);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                list = new UsuarioViewModel
                                {
                                    idUsuario = Convert.ToInt32(reader["idusuario"]),
                                    nombreUsuario = reader["nombreUsuario"].ToString(),
                                    contraseniaUsuario = reader["contraseniaUsuario"].ToString(),
                                   
                                };
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
        public bool EditarUsuario(UsuarioModel User_)
        {

            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("modificar_usuario", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", User_.idUsuario);
                        command.Parameters.AddWithValue("@contrasenia",User_.contraseniaUsuario);
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

        public bool Eliminar_Usuario(int id_usuario)
        {
            bool flag;
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminar_usuario", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id_usuario);
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

    }
}
