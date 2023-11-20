using Microsoft.AspNetCore.Mvc;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class SolicitudesController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro miembro = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            List<Solicitud> solicitudes = redsocial.BuscarSolicitudesporMiembro(miembro);
            ViewBag.Solicitudes = solicitudes;
            return View();
        }

        public IActionResult MisSolicitudesEnviadas()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro miembro = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            List<Solicitud> solicitudes = redsocial.GetTodasLasSolicitudesDe(miembro);
            ViewBag.Solicitudes = solicitudes;
            return View("Index");
        }

        public IActionResult EnviarSolicitud(string correosolicitado)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Miembro solicitante = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                Miembro solicitado = redsocial.BuscarMiembro(correosolicitado);
                if(!redsocial.SolicitudEnviada(solicitante,solicitado))
                    redsocial.AltaRelacion(solicitante, solicitado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return Redirect($"/Usuarios/Perfil?correo={correosolicitado}");
        }

        public IActionResult EliminarAmistad(string correo)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Miembro solicitado = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                Miembro solicitante = redsocial.BuscarMiembro(correo);
                redsocial.RechazarRelacion(solicitante,solicitado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return Redirect($"/Usuarios/Perfil?correo={correo}");
        }

        public IActionResult AceptarSoli(string correo)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Miembro solicitado = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                Miembro solicitante = redsocial.BuscarMiembro(correo);
                redsocial.AceptarRelacion(solicitante, solicitado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Solicitudes");
        }

        public IActionResult RechazarSoli(string correo)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Miembro solicitado = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                Miembro solicitante = redsocial.BuscarMiembro(correo);
                redsocial.RechazarRelacion(solicitante, solicitado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Solicitudes");
        }

    }
}
