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

        public IActionResult Banear(int idpost)
        {
            if (HttpContext.Session.GetString("rol") != "admin")
                return RedirectToAction("Login", "Home");
            try
            {
                Post post = redsocial.GetPostporId(idpost);
                post.IsCensurado = true;
            }catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Desbanear(int idpost)
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
            return RedirectToAction("Index");
        }

    }
}
