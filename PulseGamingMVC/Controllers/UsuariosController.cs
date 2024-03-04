using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;

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
        public async Task<IActionResult> RegistrarUsuario(string nombre, string apellidos, string email, string password, int telefono)
        {
            await this.repo.RegisterUser(nombre, apellidos, email, password, telefono);
            ViewData["MENSAJE"] = "Usuario registrado correctamente. " +
                " Hemos enviado un mail para activar su cuenta";
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
                return View(user);
            }
        }

    }
}
