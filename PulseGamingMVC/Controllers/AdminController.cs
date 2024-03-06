using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;

namespace PulseGamingMVC.Controllers
{
    public class AdminController : Controller
    {
        private IRepositoryJuegos repoGame;
        private RepositoryUsuarios repoUsu;

        public AdminController(IRepositoryJuegos repoGame, RepositoryUsuarios repoUsu)
        {
            this.repoGame = repoGame;
            this.repoUsu = repoUsu;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult UsuariosView()
        {
            List<Usuario> usuarios = this.repoUsu.GetUsuarios();
            return View(usuarios);
        }

        public IActionResult JuegosGeneros()
        {
            List<Juego> juegos = this.repoGame.GetJuegos();
            return View(juegos);
        }
    }
}
