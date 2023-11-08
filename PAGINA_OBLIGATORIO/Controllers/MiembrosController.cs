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
