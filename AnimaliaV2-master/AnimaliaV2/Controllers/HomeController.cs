using AnimaliaV2.Data;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using iText.Kernel.Geom;
using System.Security.Claims;

namespace AnimaliaV2.Controllers
{
    public class HomeController : Controller
    {
        public readonly HomeManejador _manejadorHome;

        public HomeController(MySqlConnection connection)
        {
            _manejadorHome = new HomeManejador(connection);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult reportes()
        {
            return View();
        }



        public IActionResult prueba(int a, string fecha)
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);

            //DataTable dt = new DataTable();
            //string templatePath = "";

            var htmlOutput = "";
            switch (a)
            {
                case 1:
                    htmlOutput = _manejadorHome.reporte_mascotas_adoptadas(); 
                    break;
                case 2:
                    htmlOutput = _manejadorHome.reporte_mascotas_estado_adopcion();
                    break;
                case 3:
                    htmlOutput = _manejadorHome.reporte_mascotas_noadoptadas();
                    break;
                case 4:
                    htmlOutput = _manejadorHome.reporte_cantidad_mascotas_adoptadas();
                    break;
                case 5:
                    htmlOutput = _manejadorHome.reporte_mis_mascotas(idUsuario);
                    break;
                case 6:
                    htmlOutput = _manejadorHome.reporte_mis_mascotas_filtroFecha(idUsuario, fecha);
                    break;
                case 7:
                    htmlOutput = _manejadorHome.reporte_mis_mascotas_adoptadas(idUsuario);
                    break;
                case 8:
                    htmlOutput = _manejadorHome.reporte_mis_mascotas_NoAdoptadas(idUsuario);
                    break;
                case 9:
                    htmlOutput = _manejadorHome.reporte_mascota_filtro_especie();
                    break;
                case 10:
                    htmlOutput = _manejadorHome.reporte_mascota_filtro_raza();
                    break;
            }



            //Hastaaaaaa---------------------------------------------------------------------
            var outputStream = new MemoryStream();
            var properties = new ConverterProperties();
            properties.SetBaseUri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css"));

            var pdfWriter = new PdfWriter(outputStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            pdfDocument.SetDefaultPageSize(new PageSize(PageSize.A4).Rotate());
            var document = HtmlConverter.ConvertToDocument(htmlOutput, pdfDocument, properties);

            document.Close();

            // Abrir el PDF en una nueva ventana del navegador
            var fileBytes = outputStream.ToArray();
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                Inline = true,  // Abrir en línea en lugar de descargar
                FileName = "documento.pdf"
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return File(fileBytes, "application/pdf");
        }


        

        public IActionResult permisos()
        {

           

            return View();
        }




    }



}
