using Microsoft.AspNetCore.Mvc;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class SolicitudesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
