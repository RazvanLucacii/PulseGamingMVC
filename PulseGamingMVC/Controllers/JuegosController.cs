using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Extensions;
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

        public IActionResult Details(int? IdJuego)
        {
            if (IdJuego != null)
            {
                var juego = this.repo.FindJuego(IdJuego.Value);
                if (juego != null)
                {
                    var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

                    // Verificar si el juego ya está en el carrito
                    var existingItem = carrito.FirstOrDefault(item => item.IdJuego == IdJuego.Value);
                    if (existingItem != null)
                    {
                        existingItem.Cantidad++;
                    }
                    else
                    {
                        carrito.Add(new Carrito
                        {
                            IdJuego = juego.IdJuego,
                            NombreJuego = juego.NombreJuego,
                            PrecioJuego = juego.PrecioJuego,
                            Cantidad = 1
                        });

                        TempData["SuccessMessage"] = "Juego añadido al carrito";
                    }

                    HttpContext.Session.SetObject("CARRITO", carrito);
                    ViewData["MENSAJE"] = "Juegos en el carrito: " + carrito.Count;
                }
            }

            Juego juegoDetalle = this.repo.FindJuego(IdJuego.Value);
            return View(juegoDetalle);
        }

        public IActionResult Carrito()
        {
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();
            return View(carrito);
        }

    }
}
