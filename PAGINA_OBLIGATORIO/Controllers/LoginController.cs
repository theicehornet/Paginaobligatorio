using Microsoft.AspNetCore.Mvc;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
