using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Extensions;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using System;

namespace PulseGamingMVC.Controllers
{
    public class JuegosController : Controller
    {
        private IRepositoryJuegos repo;

        public JuegosController(IRepositoryJuegos repo)
        {
            this.repo = repo;
        }

        public IActionResult Inicio()
        {
            return View();
        }

        public async Task<IActionResult> Games(string precio, string search, int? posicion)
        {
            ViewData["PRECIO"] = precio;
            if (posicion == null)
            {
                posicion = 1;
            }
            int numeroRegistros = await this.repo.GetNumeroJuegosAsync();
            ViewData["REGISTROS"] = numeroRegistros;
            ViewData["POSICION"] = posicion;

            if (precio == "desc")
            {
                var juegosDesc = this.repo.GetJuegosPrecioDesc();
                return View(juegosDesc);
            }
            else if(precio == "asc")
            {
                var juegosasc = this.repo.GetJuegosPrecioAsce();
                return View(juegosasc);
            }
            else
            {
                List<Juego> juegos = await this.repo.GetGrupoJuegosAsync(posicion.Value);
                return View(juegos);
            }

        }

        public IActionResult Details(int IdJuego)
        {
            Juego juegoDetalle = this.repo.FindJuego(IdJuego);
            return View(juegoDetalle);
        }

        public async Task<IActionResult> ListarJuegosCategorias(int idgenero)
        {
            List<Juego> juegosCategorias = this.repo.GetJuegosGeneros(idgenero);
            return View(juegosCategorias);
        }

        [HttpPost]
        public IActionResult AñadirCarrito(int? IdJuego)
        {
            var user = HttpContext.Session.GetString("USUARIO");
            if (user == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            else
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
            }
            Juego juegoDetalle = this.repo.FindJuego(IdJuego.Value);
            return RedirectToAction("Details", juegoDetalle);
        }

        [HttpPost]
        public IActionResult AñadirAlCarritoDesdeLista(int? idJuego)
        {
            var user = HttpContext.Session.GetString("USUARIO");
            if(user == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            else
            {
                if (idJuego != null)
                {
                    var juego = repo.FindJuego(idJuego.Value);
                    if (juego != null)
                    {
                        var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

                        // Verificar si el juego ya está en el carrito
                        var existingItem = carrito.FirstOrDefault(item => item.IdJuego == idJuego.Value);
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

                            ViewData["SuccessMessage"] = "Juego añadido al carrito";
                        }

                        HttpContext.Session.SetObject("CARRITO", carrito);
                        ViewData["MENSAJE"] = "Juegos en el carrito: " + carrito.Count;
                    }
                }
            }

            return RedirectToAction("Games");
        }

        public IActionResult Carrito()
        {
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();
            return View(carrito);

        }

        public IActionResult Pedidos()
        {
            var pedido = HttpContext.Session.GetObject<Pedido>("PEDIDO");
            return View(pedido);
        }

        public IActionResult FinalizarPedido(string ciudad, string pais)
        {
            // Obtener el carrito de la sesión
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            // Calcular el total del pedido
            double total = carrito.Sum(item => item.PrecioJuego * item.Cantidad);

            // Crear el pedido
            Pedido pedido = new Pedido
            {
                FechaPedido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Ciudad = ciudad,
                Pais = pais, 
                IDUsuario = 1, 
                Total = total,
                TotalProducto = string.Join(",", carrito.Select(item => $"{item.NombreJuego} ({item.Cantidad})"))
            };

            HttpContext.Session.SetObject("PEDIDO", pedido);

            HttpContext.Session.Remove("CARRITO");

            return View(pedido);
        }

        [HttpPost]
        public IActionResult IncrementarCantidad(int idJuego)
        {

            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();
 
            var juegoEnCarrito = carrito.FirstOrDefault(item => item.IdJuego == idJuego);

            if (juegoEnCarrito != null)
            {
                juegoEnCarrito.Cantidad++;
            }

            HttpContext.Session.SetObject("CARRITO", carrito);

            return PartialView("_Carrito", carrito);
        }

        [HttpPost]
        public IActionResult DecrementarCantidad(int idJuego)
        {

            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            var juegoEnCarrito = carrito.FirstOrDefault(item => item.IdJuego == idJuego);

            if (juegoEnCarrito != null)
            {
                juegoEnCarrito.Cantidad--;
            }

            HttpContext.Session.SetObject("CARRITO", carrito);

            return PartialView("_Carrito", carrito);
        }

        public IActionResult EliminarJuego(int idJuego)
        {
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            var juegoAEliminar = carrito.FirstOrDefault(item => item.IdJuego == idJuego);
            if (juegoAEliminar != null)
            {
                carrito.Remove(juegoAEliminar);
                HttpContext.Session.SetObject("CARRITO", carrito);
                TempData["SuccessMessage"] = "Juego eliminado del carrito";
            }

            return PartialView("_Carrito", carrito);
        }
    }
}
