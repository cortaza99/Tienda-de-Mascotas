using AnimaliaV2.Controllers;
using AnimaliaV2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Text;


namespace AnimaliaV2.Data
{
    public class MascotasManejador
    {
        public MySqlConnection _connection;

        public MascotasManejador(MySqlConnection connection) {
            _connection = connection;
        }

        public bool GuardarMascota(RegistroMedicoMascotaViewModel list ,int idUsuario)
        {
            MascotaModel mascota = list.Mascota;
            RegistroMedicoVeterinarioModel registroMedicoVeterinario = list.registro;
            bool flag;
            int mascotaRescatada=(Convert.ToBoolean(mascota.mascota_rescatada))?1:0;
            int condicionEspecial= (Convert.ToBoolean(registroMedicoVeterinario.condicionEspecial)) ? 1 : 0;
            var datosJson1 = list.Antecedentes;
            var datosJson2 = list.VacunasAnimal;
            string rutaSistema = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","imgMascotas") ;  //ruta wwwroot/imgMascotas
            string nombreArchivo=mascota.nombreImagen;
            try
            {
                using(_connection)
                {
                    _connection.Open();
                    if (nombreArchivo != null && nombreArchivo.Length > 0)
                    {
                        gurdarImagen(rutaSistema,nombreArchivo);
                        eliminarImagenTemporal(nombreArchivo);
                    }
                    else
                    {
                        nombreArchivo = "uqWHfRBhFmg2DUgkaVkG.png";
                    }

                    using (MySqlCommand command = new MySqlCommand("insertar_mascotas", _connection)) {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@descrip", mascota.descripcionMascota);
                        command.Parameters.AddWithValue("@alt", mascota.alturaMascota);
                        command.Parameters.AddWithValue("@peso", mascota.pesoMascota);
                        command.Parameters.AddWithValue("@color", mascota.colorMascota);
                        command.Parameters.AddWithValue("@nac", mascota.nacimientoMascota);
                        command.Parameters.AddWithValue("@nombre", mascota.nombreMascota);
                        command.Parameters.AddWithValue("@rescatada", mascotaRescatada);
                        command.Parameters.AddWithValue("@img", nombreArchivo);
                        command.Parameters.AddWithValue("@fk_sexo", mascota.fk_idSexoAnimal);
                        command.Parameters.AddWithValue("@fk_user", idUsuario);
                        command.Parameters.AddWithValue("@fk_raza", mascota.fk_idRaza);
                        command.Parameters.AddWithValue("@fk_municipio", mascota.fk_idMunicipio);
                        command.Parameters.AddWithValue("@comida", registroMedicoVeterinario.dieta);
                        command.Parameters.AddWithValue("@especial", condicionEspecial);
                        command.Parameters.AddWithValue("@descripciones", datosJson1);
                        command.Parameters.AddWithValue("@vacunas", datosJson2);
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

        public async Task<bool> gurdarImagenTemporalmente(string rutaArchivo, string rutaSistema, IFormFile a) {
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await a.CopyToAsync(stream);
            }

            // hacer algo con la ruta del archivo...
            File.Move(rutaArchivo, rutaSistema);
            return true;
        }

        public bool gurdarImagen(string rutaArchivo, string nombreArchivo)
        {

            // Obtener la imagen desde la ruta especificada
            byte[] imageBytes = File.ReadAllBytes(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Data", "TempFiles"), nombreArchivo));

            // Crear una instancia de la clase Image a partir de los bytes de la imagen
            using (var image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes)))
            {
                

                string rutaSistema = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgMascotas");

                string extensionFile= Path.GetExtension(nombreArchivo).ToLower();

                string newFilePath = Path.Combine(rutaSistema, nombreArchivo);

                // Guardar la imagen en el directorio nuevo en el formato de archivo correspondiente a la extensión del archivo
                switch (extensionFile)
                {
                    case ".bmp":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".gif":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case ".ico":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Icon);
                        break;
                    case ".jpg":
                    case ".jpeg":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".tif":
                    case ".tiff":
                        image.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    default:
                        throw new NotSupportedException($"La extensión de archivo {extensionFile} no es compatible.");
                }
            }
                return true;
        }

        public bool eliminarImagenTemporal(string nombreImagen) {
            string rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TempFiles",nombreImagen); ;
            if (File.Exists(rutaImagen))
            {
                File.Delete(rutaImagen);
            }
            return true;
        }


        public List<MascotaModel> listarMascotas()
        {
            List<MascotaModel> list = new List<MascotaModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ver_mascotas", _connection))
                    {
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
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }

        public InfoDetalladaViewModel LeerInformacionDetalladaMascota(int idMascota) {
            InfoDetalladaViewModel vistaModelo= new InfoDetalladaViewModel();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("buscar_informacion_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@valor_id", idMascota);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vistaModelo.Mascota = new MascotaModel
                                {
                                    idMascota = Convert.ToInt32(reader["idMascota"]),
                                    descripcionMascota = reader["descripcionMascota"].ToString(),
                                    alturaMascota = Convert.ToInt32(reader["alturaMascota"]),
                                    pesoMascota = Convert.ToInt32(reader["pesoMascota"]),
                                    colorMascota = reader["colorMascota"].ToString(), 
                                    nombreImagen = reader["imgUbicacion"].ToString(),
                                    nacimientoMascota = reader["nacimientoMascota"].ToString(), 
                                    mascota_rescatada = reader["Mascota Destacada"].ToString() 
                                };
                                vistaModelo.dieta = reader["dieta"].ToString();
                                vistaModelo.raza = reader["razaMascota"].ToString(); 
                                vistaModelo.especie = reader["especieMascota"].ToString(); 
                                vistaModelo.ubicacion = reader["Ubicación"].ToString(); 
                                vistaModelo.condicionEspecial = reader["Condición Especial"].ToString(); 
                            }
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand("buscar_antecedentes_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@valor_id", idMascota);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<AntecedenteMedicoModel> antecedentes = new List<AntecedenteMedicoModel>();
                            while (reader.Read())
                            {
                                antecedentes.Add(new AntecedenteMedicoModel
                                {
                                    descripcionAntecedente = reader["descripcionAntecedente"].ToString()
                                });
                            }
                            vistaModelo.antecedentes= antecedentes;
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand("buscar_vacunas_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@valor_id", idMascota);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<VacunaModel> vacunas = new List<VacunaModel>();
                            while (reader.Read())
                            {
                                vacunas.Add(new VacunaModel
                                {
                                    nombreVacuna = reader["nombreVacuna"].ToString()
                                });
                            }
                            vistaModelo.vacunas = vacunas;
                        }
                    }

                    _connection.Close();
                }
                
                return vistaModelo;


            }
            catch (Exception ex)
            {
                return vistaModelo;
            }
        }



        public RegistroMascotaViewModel ListarVistaRegistro() {

            RegistroMascotaViewModel vistaRegistroModel = new RegistroMascotaViewModel();   

            try
            {
                using (_connection)
                {
                    _connection.Open();

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM especieMascota", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<EspecieMascotaModel> especies = new List<EspecieMascotaModel>();
                            while (reader.Read())
                            {
                                

                                especies.Add(new EspecieMascotaModel
                                {
                                    idEspecieMascota = Convert.ToInt32(reader["idEspecieMascota"]),
                                    especieMascota = reader["especieMascota"].ToString()
                                });
                                
                            }
                            vistaRegistroModel.especies = especies;
                        }
                    }


                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM departamento", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                          List<DepartamentoModel> departamentos = new List<DepartamentoModel>();
                            while (reader.Read())
                            {

                                departamentos.Add(new DepartamentoModel
                                {
                                    IdDepartamento = Convert.ToInt32(reader["idDepartamento"]),
                                    nombreDepartamento = reader["nombreDepartamento"].ToString()
                                });
                            }
                                vistaRegistroModel.departamentos = departamentos;
                        }
                    }
                    _connection.Close();
                }
                return vistaRegistroModel;
            }
            catch (Exception ex)
            {
                return vistaRegistroModel;
            }



        }

        public List<MunicipioModel> ListarMunicipios(int idDepartamento) {
            List<MunicipioModel> municipios1 = new List<MunicipioModel>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("ver_municipios", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id",idDepartamento);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                municipios1.Add(new MunicipioModel
                                {
                                    IdMunicipio = Convert.ToInt32(reader["idMunicipio"]),
                                    nombreMunicipio = reader["nombreMunicipio"].ToString(),
                                    fk_departamento = Convert.ToInt32(reader["fk_idDepartamento"])
                                });
                            }
                        }
                    }


                    _connection.Close();
                }
                return municipios1;
            }
            catch (Exception ex)
            {
                return municipios1;
            }


        }


        public List<RazaMascotaModel> ListarRazas(int idEspecie)
        {
            List<RazaMascotaModel> razas = new List<RazaMascotaModel>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("ver_razas", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", idEspecie);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                razas.Add(new RazaMascotaModel
                                {
                                    idRazaMascota = Convert.ToInt32(reader["idRazaMascota"]),
                                    razaMascota = reader["razaMascota"].ToString(),
                                    fk_idEspecieMascota = Convert.ToInt32(reader["fk_idEspecieMascota"])
                                });
                            }
                        }
                    }


                    _connection.Close();
                }
                return razas;
            }
            catch (Exception ex)
            {
                return razas;
            }


        }

        public RegistroMedicoMascotaViewModel ListarVistaRegistroMedico()
        {

            RegistroMedicoMascotaViewModel vistaRegistroMedicoModel = new RegistroMedicoMascotaViewModel();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM vacuna", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<VacunaModel> vacunas = new List<VacunaModel>();
                            while (reader.Read())
                            {
                                vacunas.Add(new VacunaModel
                                {
                                    idVacuna = Convert.ToInt32(reader["idVacuna"]),
                                    nombreVacuna = reader["nombreVacuna"].ToString()
                                });

                            }
                            vistaRegistroMedicoModel.vacunas = vacunas;
                        }
                    }

                    _connection.Close();
                }
                return vistaRegistroMedicoModel;
            }
            catch (Exception ex)
            {
                return vistaRegistroMedicoModel;
            }



        }

        public bool generarSolicitudMascota(int id, int idUsuario) {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    //  Verificar si el usuario es el mismo que registró la mascota

                    using (MySqlCommand verificarCommand = new MySqlCommand("dueño_solicitud_adopcion", _connection))
                    {
                        verificarCommand.CommandType = CommandType.StoredProcedure;
                        verificarCommand.Parameters.AddWithValue("@idMascota", id);
                        verificarCommand.Parameters.AddWithValue("@fk_usuario", idUsuario);

                        int count = Convert.ToInt32(verificarCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            return false;
                        }
                    }

                    // Verificar si el usuario ya tiene una solicitud para esa mascota

                    using (MySqlCommand verificarCommand = new MySqlCommand("verificar_solicitud_adopcion", _connection))
                    {
                        verificarCommand.CommandType = CommandType.StoredProcedure;
                        verificarCommand.Parameters.AddWithValue("@idMascota", id);
                        verificarCommand.Parameters.AddWithValue("@fk_usuario", idUsuario);

                        int count = Convert.ToInt32(verificarCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            return false;
                        }
                    }

                    // Si no existe una solicitud previa, generar la nueva solicitud
                    using (MySqlCommand command = new MySqlCommand("solicitud_adopcion", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idMascota", id);
                        command.Parameters.AddWithValue("@fk_usuario", idUsuario);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            return false;
            }
            
        }



        public bool registrarEspecie(string especie) {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("registrarEspecie", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@especie", especie);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }

        public List<EspecieMascotaModel> ListarEspecies()
        {
            List<EspecieMascotaModel> especies = new List<EspecieMascotaModel>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ver_especies", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                especies.Add(new EspecieMascotaModel{
                                    idEspecieMascota = Convert.ToInt32(reader["idEspecieMascota"]),
                                    especieMascota = reader["nombreEspecie"].ToString()
                                });
                            }
                        }
                    }


                    _connection.Close();
                }
                return especies;
            }
            catch (Exception ex)
            {
                return especies;
            }


        }



        public bool eliminarEspecie(int idEspecie)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminarEspecie", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@especie", idEspecie);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }

        //
        public RegistroRazaViewModel ListarRazasRegistroRaza()
        {
            RegistroRazaViewModel modeloVista = new RegistroRazaViewModel();
            
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ver_tabla_razas", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<RazaMascotaModel> razas = new List<RazaMascotaModel>();
                            List<EspecieMascotaModel> especies = new List<EspecieMascotaModel>();
                            while (reader.Read())
                            {
                                razas.Add(new RazaMascotaModel
                                {
                                    idRazaMascota = Convert.ToInt32(reader["idRaza"]),
                                    razaMascota = reader["nombreRaza"].ToString()
                                });

                                especies.Add(new EspecieMascotaModel 
                                {
                                    especieMascota= reader["nombreEspecie"].ToString()
                                });
                            }
                            modeloVista.especiesTabla = especies;
                            modeloVista.razasTabla = razas;
                        }
                    }
                    _connection.Close();
                    modeloVista.especiesSelect = ListarEspecies();
                }
                return modeloVista;
            }
            catch (Exception ex)
            {
                return modeloVista;
            }


        }

        public bool registrarRaza(int especieSeleccionada, string nombreRaza)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("registrarRaza", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@nombreRaza", nombreRaza);
                        command.Parameters.AddWithValue("@especieSeleccionada", especieSeleccionada);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }

        public bool eliminarRaza(int idRaza)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminar_Raza", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idRaza", idRaza);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }


        public List<VacunaModel> obtenerVacunas()
        {
            List<VacunaModel> vacunas= new List<VacunaModel>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM vacuna", _connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vacunas.Add(new VacunaModel
                                {
                                    idVacuna = Convert.ToInt32(reader["idVacuna"]),
                                    nombreVacuna = reader["nombreVacuna"].ToString()
                                });

                            }
                        }
                    }
                    _connection.Close();
                }
                return vacunas;
            }
            catch (Exception ex)
            {
                return vacunas;
            }
        }

        public bool registrarVacuna(string vacuna)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("registrarVacuna", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@vacuna", vacuna);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }


        public bool eliminarVacuna(int idVacuna)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminarVacuna", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idVac", idVacuna);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }

        public ModificarRegistroVeterinarioViewModel modificarRegistroVeterinario(int id) {
            ModificarRegistroVeterinarioViewModel viewModel=new ModificarRegistroVeterinarioViewModel();
            viewModel.vacunaModels= obtenerVacunas();
            viewModel.misVacunas=new List<VacunaModel>();
            viewModel.misAntecedentes = new List<AntecedenteMedicoModel>();

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("obtenerRegistroMedicoVeterinario", _connection))
                    {
                        command.CommandType=CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                viewModel.miRegistro = new RegistroMedicoVeterinarioModel {
                                    idRegistroMedicoVeterinario = Convert.ToInt32(reader["idRegistroMedicoVeterinario"]),
                                    dieta = reader["dieta"].ToString(),
                                    condicionEspecial = reader["condicionEspecial"].ToString()
                                };

                            }
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand("buscar_antecedentes_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@valor_id", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                viewModel.misAntecedentes.Add(new AntecedenteMedicoModel {
                                    idAntecedenteMedico = Convert.ToInt32(reader["idAntecedenteMedico"]),
                                    descripcionAntecedente = reader["descripcionAntecedente"].ToString(),
                                    fechaAntecedente = reader["fechaAntecedente"].ToString()
                                });

                            }
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand("buscar_vacunas_mascota", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@valor_id", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                viewModel.misVacunas.Add(new VacunaModel
                                {
                                    idVacuna = Convert.ToInt32(reader["idVacuna"]),
                                    nombreVacuna = reader["nombreVacuna"].ToString(),
                                    fecha = reader["fechaVacunaAnimal"].ToString(),
                                    idVacunaanimal = Convert.ToInt32(reader["idVacunaAnimal"])
                                });

                            }
                        }
                    }
                    _connection.Close();
                }
                return viewModel;
            }
            catch (Exception ex)
            {
                return viewModel;
            }

        }

        public bool eliminarVacunaAnimal(int idVacuna) {

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminarVacunaAnimal", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idVacunaAnima", idVacuna);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }


        }

        public bool agregarVacunaAnimal(int idVacuna, int idRegistro)
        {

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("agregarVacunaAnimal", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idVacunaAnima", idVacuna);
                        command.Parameters.AddWithValue("@idRegistro", idRegistro);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }


        }

        public bool eliminarAntecedenteAnimal(int idAntecedente)
        {

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("eliminarAntecedenteAnimal", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idAntece", idAntecedente);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }


        }

        public bool agregarAntecedenteAnimal(string descripcionAntecedente, int idRegistro)
        {

            try
            {
                using (_connection)
                {
                    _connection.Open();
                    using (MySqlCommand command = new MySqlCommand("agregarAntecedenteAnimal", _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@descripcion", descripcionAntecedente);
                        command.Parameters.AddWithValue("@idRegistro", idRegistro);
                        command.ExecuteNonQuery();
                    }
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }


        }

    }
}

