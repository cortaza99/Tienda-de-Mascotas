using AnimaliaV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AnimaliaV2.Data;
using MySql.Data.MySqlClient;

namespace AnimaliaV2.Controllers
{
    public class AccesoController : Controller
    {

        public readonly AccesoManejador _manejadorAcceso;

        public AccesoController(MySqlConnection connection)
        {
            _manejadorAcceso = new AccesoManejador(connection);
        }

        [HttpPost] 
        public async Task<IActionResult> Index(UsuarioModel a)
        {
            
            var usuario = _manejadorAcceso.validarUsuario(a.nombreUsuario,a.contraseniaUsuario);

            if (usuario != null)
            {
                var clains = new List<Claim>();
                clains.Add(new Claim(ClaimTypes.Role,usuario.fk_idRol.ToString()));
                clains.Add(new Claim(ClaimTypes.Name, usuario.nombreUsuario.ToString()));
                clains.Add(new Claim("idUsuario", usuario.idUsuario.ToString(), ClaimValueTypes.String));
                //clains.Add(new Claim("NombreUsuario", usuario.nombreUsuario.ToString(), ClaimValueTypes.String));

                var clainsIdentity = new ClaimsIdentity(clains, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clainsIdentity));

                return RedirectToAction("Index", "Home");


            }   else{
                TempData["Message"] = "Por favor, verifica tu USUARIO y CONTRASENA";
                return RedirectToAction("IniciarSesion", "Invitado");
            }


        }



        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Invitado");
        }


    }
}
