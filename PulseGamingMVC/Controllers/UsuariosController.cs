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
        private RepositoryUsuarios repo;

        public UsuariosController(RepositoryUsuarios repo)
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
            return View();
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
                HttpContext.Session.SetInt32("UserRole", user.IDRole);

                // Redireccionar según el rol del usuario
                if (user.IDRole == 1) // Suponiendo que el ID 1 corresponde al rol de administrador
                {
                    HttpContext.Session.SetString("USUARIO", user.ToString());
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    HttpContext.Session.SetString("USUARIO", user.ToString());
                    return RedirectToAction("Home", "Juegos");
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
