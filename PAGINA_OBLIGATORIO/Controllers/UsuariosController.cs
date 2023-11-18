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
        //UN MIEMBRO PUEDE:
        //1) VER SU PERFIL(VER SUS DATOS Y LOS POSTS QUE HA SUBIDO).
        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            string correo = HttpContext.Session.GetString("correo");
            Miembro logueado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(logueado);
            return View(logueado);
        }

        

        public IActionResult Amigos()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro mi = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            ViewBag.Amigos = redsocial.GetAmigos(mi);
            return View();
        }
        //2) VER LOS POSTS PUBLICOS Y PRIVADOS CORRESPONDIENTES.

        //(SE ENCARGA EL POSTCONTROLLER)

        //3) HACER UN NUEVO POST (PUBLICO O PRIVADO).

        //(SE ENCARGA EL POSTCONTROLLER)

        //4) HACER UN COMENTARIO A UN POST.

        //(SE ENCARGA EL POSTCONTROLLER)

        //5) ENVIAR UNA SOLICITUD A LAS PERSONAS.

        //SE ENCARGA SOLICITUDESCONTROLLER.

        //6) VER LAS SOLICITUDES QUE TIENE PENDIENTE COMO SOLICITADO Y COMO SOLICITANTE, SI COMO ESTA ULTIMA NO PUEDE ACEPTARLAS
        //   (PORQUE ES EL SOLICUTANTE).

        //SE ENCARGA SOLICITUDESCONTROLLER.

        //7) REACCIONAR A UN POST (LIKE/DISLIKE).

        //(SE ENCARGA EL POSTCONTROLLER)

        //8) REACCIONAR A UN COMENTARIO (LIKE/DISLIKE).

        //(SE ENCARGA EL POSTCONTROLLER)

        //9) VER PERFILES DE OTROS MIEMBROS.

        public IActionResult PerfilMiembro(string correo)
        {
            if (HttpContext.Session.GetString("rol") == null)
                return RedirectToAction("Login", "Home");
            Miembro buscado = redsocial.BuscarMiembro(correo);
            ViewBag.Posts = redsocial.BuscarPostsdeMiembro(buscado);
            return View("Perfil", buscado);
        }

        //10) DADO A UN TEXTO Y UN NUMERO QUE SERA LA ACEPTACION DE LAS PUBLICACIONES, SE BUSCARA LOS POST Y COMENTARIOS
        //    QUE COINCIDAN QUE TENGAN EL TEXTO EN SU CONTENIDO Y SU ACEPTACION SEA IGUAL O MAYOR A LA DEL PARAMETRO INGRESADO.

        //SE ENCARGA POSTCONTROLLER

        #endregion

        public IActionResult BuscarMiembrosPorNombre(string nombre = "",string apellido = "")
        {
            ViewBag.MiembrosBuscados = redsocial.GetMiembrosPorNombre(nombre, apellido);
            return View("VerMiembros");
        }

    }
}
