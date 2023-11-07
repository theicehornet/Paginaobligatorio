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
    }
}
