using Microsoft.AspNetCore.Mvc;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class PostsController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;
        public IActionResult Index()
        {
            ViewBag.Posts = redsocial.GetPostPublicos();
            return View();
        }

        public IActionResult VerPosts()
        {
            ViewBag.Posts = redsocial.GetPostVisiblesDeMiembro(null);
            return View();
        }

    }
}
