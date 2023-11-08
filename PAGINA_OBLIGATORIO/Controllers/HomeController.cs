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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            email = email.ToLower();
            try
            {
                Usuario user = redsocial.AuthenticateUsuario(email, password);
                if(user.Rol() == "admin")
                {
                    HttpContext.Session.SetString("Nombre", "Administrador");
                }else if(user.Rol() == "miembro")
                {
                    Miembro miembro = (Miembro)user;
                    HttpContext.Session.SetString("Nombre", miembro.NombreCompleto());
                }
                HttpContext.Session.SetString("rol",user.Rol());
                HttpContext.Session.SetString("correo", user.Email);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
            
        }

        [HttpGet]
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