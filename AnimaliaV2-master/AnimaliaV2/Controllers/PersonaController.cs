using AnimaliaV2.Data;
using AnimaliaV2.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Security.Claims;

namespace AnimaliaV2.Controllers
{
    public class PersonaController : Controller
    {
        public readonly PersonaManejador _personaManejador;

        public PersonaController(MySqlConnection connection)
        {
            _personaManejador = new PersonaManejador(connection);
        }
        public IActionResult misSolicitudes()
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            List<SolicitudAdopcionViewModel> list = _personaManejador.listarmisSolicitudes(idUsuario);
            return View(list);
        }

        [HttpPost]
        public IActionResult eliminarSolicitud(int id)
        {

            _personaManejador.EliminarSolicitud(id);
            return RedirectToAction("misSolicitudes");
        }

        public IActionResult GestionarSolicitudes()
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            List<GestionarSolicitudViewModel> list = _personaManejador.gestionar_solicitudes(idUsuario);
            return View(list);
        }

        [HttpPost]
        public IActionResult denegarSolicitud(int id_denegar)
        {
            _personaManejador.denegarSolicitud(id_denegar);
            return RedirectToAction("GestionarSolicitudes");
        }

        [HttpPost]
        public IActionResult AprobarSolicitud(int id_aprobar, int id_mascota)
        {
            _personaManejador.AprobarSolicitud(id_aprobar, id_mascota);
            return RedirectToAction("GestionarSolicitudes");
        }

        public IActionResult GestionarDonaciones()
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            List<GestionAdopcionViewModel> list = _personaManejador.gestionar_donaciones(idUsuario);
            return View(list);
        }

        [HttpPost]
        public IActionResult eliminar_solicitud_adopcion(int id_solicitud, int id_mascota)
        {
            _personaManejador.eliminar_adopcion(id_solicitud, id_mascota);
            return RedirectToAction("GestionarDonaciones");
        }

        [HttpPost]
        public IActionResult donarMascota(int id_solicitud, int id_mascota)
        {
            _personaManejador.donarMascota(id_solicitud, id_mascota);
            return RedirectToAction("GestionarDonaciones");
        }

        public IActionResult misMascotas()
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            List<MisMascotasModel> list = _personaManejador.misMascotas(idUsuario);
            return View(list);
        }
        public IActionResult EliminarMascota(int id_mascota)
        {
            _personaManejador.EliminarMascota(id_mascota);
            return RedirectToAction("misMascotas");
        }
        public IActionResult ModificarPersona()
        {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            PersonaModel list = _personaManejador.ConsultarPersona(idUsuario);
            return View(list);
        }

        [HttpPost]
        public IActionResult ModificarPersona(PersonaModel persona)
        {
            _personaManejador.ModificarPersona(persona);
            return RedirectToAction("ModificarPersona");
        }
        public IActionResult ListadoPersonas()
        { 
            List<PersonaModel> list = _personaManejador.ListadoPersonas();
            return View(list);
        }

       
        public IActionResult EditarPersona(int id_persona)
        {
            PersonaModel list = _personaManejador.ConsultarIdPersona(id_persona);
            return View(list);
        }

        [HttpPost]
        public IActionResult Editar_Persona(PersonaModel persona)
        {
            _personaManejador.EditarPersona(persona);
            return RedirectToAction("ListadoPersonas");
        }

        public IActionResult listadoUsuarios()
        {
            List<UsuarioViewModel> list = _personaManejador.ListadoUsuarios();
            return View(list);
        }

        [HttpGet]
        public IActionResult Modificar_Usuario(int id_usuario)
        {
            UsuarioViewModel list = _personaManejador.ConsultarUsuario(id_usuario);
            return View(list);
        }

        [HttpPost]
        public IActionResult Modificar_Usuario(UsuarioModel user_)
        {
            _personaManejador.EditarUsuario(user_);
            return RedirectToAction("listadoUsuarios");
        }

        public IActionResult Eliminar_Usuario(int id_usuario)
        {
            _personaManejador.Eliminar_Usuario(id_usuario);
            return RedirectToAction("listadoUsuarios");
        }
        public IActionResult solicitudes()
        {

            return View();
        }


        public IActionResult gestionarPersonas()
        {

            return View();
        }
    }
}
