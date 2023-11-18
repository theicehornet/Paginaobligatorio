using Microsoft.AspNetCore.Mvc;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class PostsController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;
        public IActionResult Index()
        {
            string? roluser = HttpContext.Session.GetString("rol");
            if (roluser == null)
            {
                ViewBag.Posts = redsocial.GetPostPublicos();
            }
            else if(roluser == "admin")
            {
                ViewBag.Posts = redsocial.CopiadePosts();
            }
            else
            {
                string correo = HttpContext.Session.GetString("correo");
                Miembro miembro = redsocial.BuscarMiembro(correo);
                ViewBag.Posts = redsocial.GetPostVisiblesDeMiembro(miembro);
            }
            return View(); 
        }

        public IActionResult FiltarPostPor(string texto, int aceptacion)
        {
            Miembro unm = null;
            try
            {
                unm = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            }catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            List<Post> posts = redsocial.BuscarPostsPorTextoyAceptacion(texto, aceptacion, unm);
            ViewBag.Posts = posts;
            List<Comentario> comentarios = redsocial.BuscarComentarioPorTextoyAceptacion(texto, aceptacion, unm);
            ViewBag.Comentarios = comentarios;
            return View("Filtrado");
        }

        public IActionResult BanearPost(int idpost)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            try
            {
                Post post = redsocial.GetPostporId(idpost);
                post.IsCensurado = true;
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }
            return RedirectToAction("Index", "Post");
        }

        public IActionResult DesbanearPost(int idpost)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            try
            {
                Post post = redsocial.GetPostporId(idpost);
                post.IsCensurado = false;
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }
            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public IActionResult PublicarPost(string titulo, string contenido, string imagen, bool isprivado)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro unm = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            try
            {
                redsocial.AltaPost(titulo, unm, contenido, imagen, isprivado);
                return RedirectToAction("Index", "Post");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public IActionResult PublicarPost()
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            return View();
        }

        //4) HACER UN COMENTARIO A UN POST.
        public IActionResult RealizarComentario(int idpost, string titulo, string contenido)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            Miembro autor = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
            try
            {
                Post post = redsocial.BuscarPostPorID(idpost);
                redsocial.RealizarComentarioaPost(post, titulo, contenido, autor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult ReaccionarPost(int idpost, int reaccion)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Post p = redsocial.GetPostporId(idpost);
                Miembro Autor = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                redsocial.RealizarReaccionPost(Autor, Convert.ToBoolean(reaccion), p);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult ReaccionarComentario(int idpost, int idcomentario, int reaccion)
        {
            if (HttpContext.Session.GetString("rol") != "miembro")
                return RedirectToAction("Login", "Home");
            try
            {
                Post p = redsocial.GetPostporId(idpost);
                Miembro Autor = redsocial.BuscarMiembro(HttpContext.Session.GetString("correo"));
                redsocial.RealizarReaccionComentario(Autor, Convert.ToBoolean(reaccion), p.BuscarComentario(idcomentario));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Posts");
        }
    }
}
