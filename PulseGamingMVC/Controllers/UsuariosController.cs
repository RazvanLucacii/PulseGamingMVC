using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using PulseGamingMVC.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http.HttpResults;


namespace PulseGamingMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private IRepositoryUsuarios repo;

        public UsuariosController(IRepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(string nombre, string apellidos, string email, string password, int telefono, int IDRole)
        {
            await this.repo.RegisterUser(nombre, apellidos, email, password, telefono, IDRole);
            ViewData["MENSAJE"] = "Usuario registrado correctamente.";
            return RedirectToAction("Login", "Usuarios");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario user = await this.repo.LogInUserAsync(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("UserRole", user.IDRole.ToString());

                // Redireccionar según el rol del usuario
                if (user.IDRole == 1) // Suponiendo que el ID 1 corresponde al rol de administrador
                {
                    HttpContext.Session.SetObject("USUARIO", user);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    HttpContext.Session.SetObject("USUARIO", user);
                    return RedirectToAction("Inicio", "Juegos");
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USUARIO");
            return RedirectToAction("Login");
        }

    }
}
