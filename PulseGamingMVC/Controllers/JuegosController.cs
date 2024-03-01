using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;

namespace PulseGamingMVC.Controllers
{
    public class JuegosController : Controller
    {
        private IRepositoryJuegos repo;

        public JuegosController(IRepositoryJuegos repo)
        {
            this.repo = repo;
        }

        public IActionResult Home()
        {
            List<Juego> juegos = this.repo.GetJuegos();
            return View(juegos);
        }

        public IActionResult Details(int IdJuego)
        {
            Juego juego = this.repo.FindJuego(IdJuego);
            return View(juego);
        }
    }
}
