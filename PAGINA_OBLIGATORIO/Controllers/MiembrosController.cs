using Microsoft.AspNetCore.Mvc;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class MiembrosController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login","Home");
            ViewBag.Usuarios = redsocial.CopiadeListaMiembros();
            return View();
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("rol") == null)
                return RedirectToAction("Login", "Home");
            string correo = HttpContext.Session.GetString("correo");
            Miembro logueado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(logueado);
            return View(logueado);
        }

        
        public IActionResult PerfilMiembro(string correo)
        {
            if (HttpContext.Session.GetString("rol") == null)
                return RedirectToAction("Login", "Home");
            Miembro buscado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(buscado);
            return View("Perfil",buscado);
        }

        public IActionResult VisualizarMiembrosOrdenados()
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            List<Miembro> miembros = redsocial.CopiadeListaMiembros();
            miembros.Sort();
            ViewBag.Usuarios = miembros;
            return View("Index");
        }

        public IActionResult Bloquear(string email)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(email);
            mi.Bloqueado = true;
            return RedirectToAction("Index");
        }

        public IActionResult Desbloquear(string email)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(email);
            mi.Bloqueado = false;
            return RedirectToAction("Index");
        }
    }
}
