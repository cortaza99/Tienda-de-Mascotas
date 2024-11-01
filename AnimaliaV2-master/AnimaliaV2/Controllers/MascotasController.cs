using AnimaliaV2.Data;
using AnimaliaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace AnimaliaV2.Controllers
{

    public class MascotasController : Controller
    {
        public readonly MascotasManejador _manejadorMascotas;

        public MascotasController(MySqlConnection connection) {
            _manejadorMascotas = new MascotasManejador(connection);
        }

        [HttpGet]

        public ActionResult ObtenerMunicipios(int deptoId)
        {
            var municipios = _manejadorMascotas.ListarMunicipios(deptoId);
            return new JsonResult(municipios);
        }

        [HttpGet]
        public ActionResult ObtenerRazas(int especieId)
        {
            var razas = _manejadorMascotas.ListarRazas(especieId);
            return new JsonResult(razas);
        }


        /*El manejador lista un modelo de continene otros modelos para la presentacion
         de la vista, entre ellos incluye la lista de especies y departamentos*/
        [Authorize(Roles = "1,2")]
        public IActionResult registrarMascota()
        {
            RegistroMascotaViewModel lista = _manejadorMascotas.ListarVistaRegistro();
            //Devuelve la lista
            return View(lista);
        }

        [HttpPost]
        public IActionResult registrarMascota(RegistroMascotaViewModel list, IFormFile archivo) {
            //Moviendo el archivo de imagen a un ruta temporal
            string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            string rutaSistema = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TempFiles");
            string rutaArchivo = Path.Combine(rutaSistema, nombreArchivo);
            ////Usando el metodo para mover la imagen
            _manejadorMascotas.gurdarImagenTemporalmente(rutaArchivo, rutaSistema, archivo);

            //if (archivo != null && archivo.Length > 0)
            //{
            //    var nombreArchivo = Path.GetTempFileName();

            //    using (var stream = new FileStream(nombreArchivo, FileMode.Create))
            //    {
            //        archivo.CopyTo(stream);
            //    }

            //    mascota.imgMascota = new FileStream(nombreArchivo, FileMode.Open);
            //}
            list.mascota.imgUbicacionExterna = rutaArchivo;
            list.mascota.nombreImagen = nombreArchivo;
            return RedirectToAction("registroMedicoVeterinario", list.mascota);
        }

        //string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(list.mascota.imgMascota.FileName);
        //string rutaSistema = Path.Combine(Directory.GetCurrentDirectory(), "Data","TempFiles");
        //string rutaArchivo = Path.Combine(rutaSistema, nombreArchivo);
        ////Usando el metodo para mover la imagen
        ////_manejadorMascotas.moverImagen(rutaArchivo,rutaSistema,list.mascota.imgMascota);



        /*La informacion del registro de la mascota es traida al registro medico
         veterinario en donde se hara otra modelo de vista que incluye otros modelos
        para vacunas y antecedentes */

        //incluir la mascota como parametro, tambien pasarla en el serializador
      
        public IActionResult registroMedicoVeterinario(MascotaModel mascota)
        {
            RegistroMedicoMascotaViewModel vistaModelo = _manejadorMascotas.ListarVistaRegistroMedico();
            string serializedModel = JsonConvert.SerializeObject(mascota);
            TempData["ModelMascota"] = serializedModel;
            return View(vistaModelo);
        }

        [HttpPost]
        public IActionResult registroMedicoVeterinario(RegistroMedicoMascotaViewModel vistaModelo) {
            string serializedModel = TempData["ModelMascota"] as string;
            MascotaModel mascota = JsonConvert.DeserializeObject<MascotaModel>(serializedModel);
            vistaModelo.Mascota = mascota;
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);
            Console.WriteLine(_manejadorMascotas.GuardarMascota(vistaModelo,idUsuario));
            return RedirectToAction("Index", "Home");
        }


        public IActionResult verMascotas()
        {
            List<MascotaModel> list = _manejadorMascotas.listarMascotas();
            return View(list);
        }


        [HttpPost]
        public IActionResult verInfoDetalladaMascota(int id)
        {
            InfoDetalladaViewModel vistaModelo = _manejadorMascotas.LeerInformacionDetalladaMascota(id);
            return View(vistaModelo);
        }

        [HttpPost]
        public IActionResult solicitarAdopcionMascota(int id) {
            ClaimsPrincipal currentUser = this.User;
            Claim idUsuarioClaim = currentUser.FindFirst("idUsuario");
            int idUsuario = Convert.ToInt32(idUsuarioClaim?.Value);

            bool solicitudGenerada = _manejadorMascotas.generarSolicitudMascota(id, idUsuario);
            if (!solicitudGenerada)
            {
                TempData["Message_solicitud"] = "No se puede solicitar la mascota.";
            }
            return RedirectToAction("Index", "Home");
        }

       

        public IActionResult registrarEspecie() {
            List<EspecieMascotaModel> especies = _manejadorMascotas.ListarEspecies();
            ViewBag.Message = TempData["Message"];
            return View(especies);
        }

        [HttpPost]
        public IActionResult registrarEspecie(string especie)
        {
            _manejadorMascotas.registrarEspecie(especie);
            return RedirectToAction("registrarEspecie","Mascotas");
        }

        [HttpPost]
        public IActionResult eliminarEspecie(int idEspecie)
        {
            if (_manejadorMascotas.eliminarEspecie(idEspecie))
            {
                return RedirectToAction("registrarEspecie", "Mascotas");
            }
            else {

                TempData["Message"] = "No se puede eliminar...Otros registros estan haciendo uso de esta informacion.";
                return RedirectToAction("registrarEspecie", "Mascotas");
            }
            
        }

        public IActionResult registrarRaza() 
        { 
            RegistroRazaViewModel vistaModelo=_manejadorMascotas.ListarRazasRegistroRaza();
            return View(vistaModelo);
        }

        [HttpPost]
        public IActionResult registrarRaza(int especieSeleccionada, string nombreRaza)
        {
            _manejadorMascotas.registrarRaza(especieSeleccionada,nombreRaza);
            return RedirectToAction("registrarRaza", "Mascotas");
        }

        [HttpPost]
        public IActionResult eliminarRaza(int razaSeleccionada)
        {
            if (_manejadorMascotas.eliminarRaza(razaSeleccionada))
            {
                return RedirectToAction("registrarRaza", "Mascotas");
            }
            else
            {

                TempData["Message"] = "No se puede eliminar...Otros registros estan haciendo uso de esta informacion.";
                return RedirectToAction("registrarRaza", "Mascotas");
            }
        }

        public IActionResult registrarVacunas()
        {
            List < VacunaModel > vacunas= _manejadorMascotas.obtenerVacunas();
            return View(vacunas);
        }

        [HttpPost]
        public IActionResult registrarVacunas(string vacuna)
        {
            _manejadorMascotas.registrarVacuna(vacuna);
            return RedirectToAction("registrarVacunas", "Mascotas");
        }

        public IActionResult eliminarVacuna(int id) {


            if (_manejadorMascotas.eliminarVacuna(id))
            {
                return RedirectToAction("registrarVacunas", "Mascotas");
            }
            else
            {

                TempData["Message"] = "No se puede eliminar...Otros registros estan haciendo uso de esta informacion.";
                return RedirectToAction("registrarVacunas", "Mascotas");
            }
        }


        public IActionResult gestionarMascotas() { 
        return View();
        }

        public IActionResult modificarRegistroMedicoVeterinario(int idRegistro) {
        ModificarRegistroVeterinarioViewModel vistaModelo = _manejadorMascotas.modificarRegistroVeterinario(idRegistro);
        return View(vistaModelo);
        }

        [HttpPost]
        public IActionResult actualizar_eliminarVacuna(int idVacuna, int idRegistro)
        {

            _manejadorMascotas.eliminarVacunaAnimal(idVacuna);

            return RedirectToAction("modificarRegistroMedicoVeterinario", "Mascotas",new { idRegistro =idRegistro});
        }

        [HttpPost]
        public IActionResult actualizar_agregarVacuna(int idVacuna, int idRegistro) { 
        
            _manejadorMascotas.agregarVacunaAnimal(idVacuna, idRegistro);

        return RedirectToAction("modificarRegistroMedicoVeterinario", new { idRegistro = idRegistro });
        }


        [HttpPost]
        public IActionResult actualizar_eliminarAntecedente(int idAntecedente, int idRegistro)
        {

            _manejadorMascotas.eliminarAntecedenteAnimal(idAntecedente);

            return RedirectToAction("modificarRegistroMedicoVeterinario", "Mascotas", new { idRegistro = idRegistro });
        }

        [HttpPost]
        public IActionResult actualizar_agregarAntecedente(string descripcionAntecedente, int idRegistro)
        {

            _manejadorMascotas.agregarAntecedenteAnimal(descripcionAntecedente, idRegistro);

            return RedirectToAction("modificarRegistroMedicoVeterinario", new { idRegistro = idRegistro });
        }


    }
}
