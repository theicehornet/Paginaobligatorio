using Microsoft.AspNetCore.Mvc;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class UsuariosController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;

        public IActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }

        #region AccionesDeAdmin
        public IActionResult VisualizarMiembros()
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login","Home");
            ViewBag.Usuarios = redsocial.CopiadeListaMiembros();
            return View("VerMiembros");
        }
        
        public IActionResult VisualizarMiembrosOrdenados()
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            List<Miembro> miembros = redsocial.CopiadeListaMiembros();
            miembros.Sort();
            ViewBag.Usuarios = miembros;
            return View("VerMiembros");
        }

        public IActionResult Bloquear(string email)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(email);
            mi.Bloqueado = true;
            return RedirectToAction("VisualizarMiembros");
        }

        public IActionResult Desbloquear(string email)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(email);
            mi.Bloqueado = false;
            return RedirectToAction("VisualizarMiembros");
        }
        #endregion
        #region AccionesDeMiembro
        public IActionResult Amigos()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            ViewBag.Amigos = redsocial.GetAmigos(mi);
            return View();
        }

        public IActionResult PerfilMiembro(string correo)
        {
            if (HttpContext.Session.GetString("rol") == null)
                return RedirectToAction("Login", "Home");
            Miembro buscado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(buscado);
            return View("Perfil", buscado);
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            string correo = HttpContext.Session.GetString("correo");
            Miembro logueado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(logueado);
            return View(logueado);
        }

        
        #endregion

    }
}
