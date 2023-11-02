using Microsoft.AspNetCore.Mvc;
using PAGINA_OBLIGATORIO.Models;
using System.Diagnostics;
using Sistema;

namespace PAGINA_OBLIGATORIO.Controllers
{
    public class HomeController : Controller
    {
        RedSocial redsocial = RedSocial.Instancia;
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
            try
            {
                redsocial.AuthenticateUsuario(email, password);
                HttpContext.Session.SetString("Email", email);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
            
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}