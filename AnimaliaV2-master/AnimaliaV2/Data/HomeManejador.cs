using AnimaliaV2.Models;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Text;


namespace AnimaliaV2.Data
{
    public class HomeManejador
    {
        public MySqlConnection _connection;

        public HomeManejador(MySqlConnection connection)
        {
            _connection = connection;
        }

        public string reporte_mascotas_adoptadas()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_mascotas_adoptadas", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();


            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla1.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("DocumentoIdentidad"),
                r2 = r.Field<string>("NombreSolicitante"),
                r3 = r.Field<string>("Direccion"),
                r4 = r.Field<string>("CorreoElectronico"),
                r5 = r.Field<string>("Telefono"),
                r6 = r.Field<string>("NombreMascota"),
                r7 = r.Field<string>("Estado"),
                r8 = r.Field<string>("FechaRegistro"),
                r9 = r.Field<string>("FechaAdopcion"),
                titulo="MASCOTAS ADOPTADAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);
          

            return template(new { registros = registrosList });
        }


        public string reporte_mascotas_estado_adopcion()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_mascota_proceso_adopcion", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla2.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("DocumentoIdentidad"),
                r2 = r.Field<string>("NombreSolicitante"),
                r3 = r.Field<string>("Direccion"),
                r4 = r.Field<string>("CorreoElectronico"),
                r5 = r.Field<string>("Telefono"),
                r6 = r.Field<string>("NombreMascota"),
                r7 = r.Field<string>("Estado"),
                r8 = r.Field<string>("FechaSolicitud"),
                titulo = "MASCOTAS EN PROCESO DE ADOPCIÓN",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }

        public string reporte_mascotas_noadoptadas()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_mascotas_noadoptadas", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla3.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("DocumentoIdentidad"),
                r2 = r.Field<string>("NombreSolicitante"),
                r3 = r.Field<string>("Direccion"),
                r4 = r.Field<string>("CorreoElectronico"),
                r5 = r.Field<string>("Telefono"),
                r6 = r.Field<string>("NombreMascota"),
                r7 = r.Field<string>("Estado"),
                r8 = r.Field<string>("FechaRegistro"),
                titulo = "MASCOTAS NO ADOPTADAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }



        public string reporte_cantidad_mascotas_adoptadas()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_cantidad_mascotas_adoptadas", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla4.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("DocumentoIdentidad"),
                r2 = r.Field<string>("NombreAdoptante"),
                r3 = r.Field<string>("Direccion"),
                r4 = r.Field<string>("CorreoElectronico"),
                r5 = r.Field<string>("Telefono"),
                r6 = r.Field<string>("CantidadMascotasAdoptadas"),
                titulo = "CANTIDAD DE MASCOTAS ADOPTADAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
               
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }

        public string reporte_mis_mascotas(int idUsuario)
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("reporte_mis_mascotas", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@xx", idUsuario);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla5.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<DateTime>("Fecha de registro").ToString("dd/MM/yyyy"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                titulo = "MIS MASCOTAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }

        public string reporte_mis_mascotas_filtroFecha(int idUsuario, string fec)
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("reporte_mis_mascotas_filtroFecha", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@xx", idUsuario);
            command.Parameters.AddWithValue("@fec", fec);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla5.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<string>("Fecha de registro"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                titulo = "FILTRO DE MASCOTAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }
        public string reporte_mis_mascotas_adoptadas(int idUsuario)
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("reporte_mis_mascotas_adoptadas", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id_usuario", idUsuario);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla6.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<string>("Fecha de registro"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                r10 = r.Field<string>("Fecha de adopcion"),
                titulo = "MASCOTAS ADOPTADAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }

        public string reporte_mis_mascotas_NoAdoptadas(int idUsuario)
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("reporte_mis_mascotas_NoAdoptadas", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id_usuario", idUsuario);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla6.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<string>("Fecha de registro"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                r10 = r.Field<string>("Fecha de adopcion"),
                titulo = "MASCOTAS NO ADOPTADAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }

        public string reporte_mascota_filtro_especie()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_mascota_filtro_especie", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla5.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<string>("Fecha de registro"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                titulo = "FILTRO DE MASCOTAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }
        public string reporte_mascota_filtro_raza()
        {
            DataTable dataTable = new DataTable();
            _connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM reporte_mascota_filtro_raza", _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            _connection.Close();
            string templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Views", "ReportesPlantilla", "plantilla5.html");
            var registrosList = dataTable.AsEnumerable().Select(r => new
            {
                r1 = r.Field<string>("Nombre de la mascota"),
                r2 = r.Field<string>("Fecha de registro"),
                r3 = r.Field<string>("Fecha de nacimiento"),
                r4 = r.Field<string>("Peso de la mascota"),
                r5 = r.Field<string>("Altura de la mascota"),
                r6 = r.Field<string>("Sexo del animal"),
                r7 = r.Field<string>("Raza-Especie"),
                r8 = r.Field<string>("Ubicacion"),
                r9 = r.Field<string>("Estado de adopcion"),
                titulo = "FILTRO DE MASCOTAS",
                fechaActual = DateTime.Now.ToString("dd/MM/yyyy")
            });

            // Lee el contenido de la plantilla HTML
            string htmlContent = System.IO.File.ReadAllText(templatePath);



            // Compilar la plantilla HTML con HandlebarsDotNet
            var template = Handlebars.Compile(htmlContent);


            return template(new { registros = registrosList });
        }


    }
}
