using AnimaliaV2.Data;
using AnimaliaV2.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace AnimaliaV2.Controllers
{
    public class InvitadoController : Controller
    {
        public readonly InvitadoManejador _manejadorinvitado;

        public InvitadoController(MySqlConnection connection)
        {
           _manejadorinvitado = new InvitadoManejador(connection);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IniciarSesion()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        public IActionResult quienesSomos()
        {
            return View();
        }
        public IActionResult verMascotas()
        {
           List<MascotaModel> list=_manejadorinvitado.listarMascotas();
            return View(list);
        }
        public IActionResult registrarPersona()
        {
           return View();
        }
        [HttpPost]
        public IActionResult registrarPersona(PersonaModel persona)
        {
            return RedirectToAction("registrarUsuario", persona);
        }

        public IActionResult registrarUsuario(PersonaModel persona)
        {
            RegistroPersonaViewModel vistaPersona = new RegistroPersonaViewModel();
            vistaPersona.Persona = persona;
            string serializedModel = JsonConvert.SerializeObject(persona);
            TempData["ModelPersona"] = serializedModel;

            return View(vistaPersona);
        }

        [HttpPost]
        public IActionResult registrarUsuario(RegistroPersonaViewModel vistaPersona)
        {
            string serializedModel = TempData["ModelPersona"] as string;
            PersonaModel persona = JsonConvert.DeserializeObject<PersonaModel>(serializedModel);
            vistaPersona.Persona = persona;
            _manejadorinvitado.GuardarPersona(vistaPersona);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult VerificarUsuario(string nombreUsuario)
        {
            // Realizar la lógica para verificar si el nombre de usuario existe en tu base de datos u otra fuente de datos
            bool nombreUsuarioExiste = _manejadorinvitado.validarUsuario(nombreUsuario);

        return Json(new { existe = nombreUsuarioExiste });
        }


    }
}
