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
            ViewData["Title"] = "Home";
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
                Usuario user = redsocial.AuthenticateUsuario(email, password);
                if(user is Administrador admin)
                {
                    HttpContext.Session.SetString("Nombre", "Administrador");
                }else if(user is Miembro miembro)
                {
                    HttpContext.Session.SetString("Nombre", miembro.NombreCompleto());
                }
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
            
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(string email, string password, string nombre, string apellido, DateTime fechanacimiento)
        {
            try
            {
                redsocial.Altamiembro(email, password, nombre, apellido, fechanacimiento);
                return Redirect($"/Home/Login");
            }catch (Exception ex)
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