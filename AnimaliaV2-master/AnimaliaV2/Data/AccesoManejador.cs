using AnimaliaV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Security.Claims;

namespace AnimaliaV2.Data
{
    public class AccesoManejador
    {
        public readonly MySqlConnection _connection;
        public AccesoManejador(MySqlConnection connection)
        {
            _connection = connection;
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
                                    fk_idRol= reader["nombrerol"].ToString()=="Cliente"?2:1
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



        public UsuarioModel validarUsuario(string usuario, string clave)
        {
            return listarUsuarios().Where(item => item.nombreUsuario == usuario && item.contraseniaUsuario == clave).FirstOrDefault();
        }

       

    }
}
