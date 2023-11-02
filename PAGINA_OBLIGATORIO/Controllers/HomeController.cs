using Microsoft.AspNetCore.Mvc;
using PAGINA_OBLIGATORIO.Models;
using System.Diagnostics;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class HomeController : Controller
    {
        RedSocial redSocial = RedSocial.Instancia;
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            return Redirect("/");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}