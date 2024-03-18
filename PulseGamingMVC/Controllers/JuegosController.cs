using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache memoryCache;

        public JuegosController(IRepositoryJuegos repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
        }

        public IActionResult Inicio()
        {
            return View();
        }

        public async Task<IActionResult> JuegosFavoritos(int? ideliminar)
        {
            var user = HttpContext.Session.GetString("USUARIO");
            if (user == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            else
            {
                    if (ideliminar != null)
                    {
                        List<Juego> juegos = this.memoryCache.Get<List<Juego>>("FAVORITOS");

                        Juego juego = juegos.FirstOrDefault(z => z.IdJuego == ideliminar.Value);

                        juegos.Remove(juego);

                        if (juegos.Count == 0)
                        {
                            this.memoryCache.Remove("FAVORITOS");
                        }
                        else
                        {
                            this.memoryCache.Set("FAVORITOS", juegos);
                        }
                    }
                return View();
            }
        }

        public async Task<IActionResult> Games(string precio, string search, int? posicion, int? idfavorito)
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
                if (idfavorito != null)
                {
                    //COMO ALMACENAMOS EN CLIENTE CACHE, VAMOS A UTILIZAR
                    //LA COLECCION DE EMPLEADOS DIRECTAMENTE
                    List<Juego> juegosFavoritos;
                    if (this.memoryCache.Get("FAVORITOS") == null)
                    {
                        //CREAMOS NUESTRA COLECCION
                        juegosFavoritos = new List<Juego>();
                    }
                    else
                    {
                        //RECUPERAMOS LOS EMPLEADOS QUE YA TENGAMOS EN CACHE
                        juegosFavoritos =
                            this.memoryCache.Get<List<Juego>>("FAVORITOS");
                    }
                    //BUSCAMOS AL EMPLEADO POR SU ID DE FAVORITO
                    Juego juego =
                        this.repo.FindJuego(idfavorito.Value);
                    juegosFavoritos.Add(juego);
                    //ALMACENAMOS LOS NUEVOS DATOS DENTRO DE CACHE
                    this.memoryCache.Set("FAVORITOS", juegosFavoritos);
                }
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

                        }

                        HttpContext.Session.SetObject("CARRITO", carrito);
                    }

                }
            }
            return RedirectToAction("Carrito");
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

                        }

                        HttpContext.Session.SetObject("CARRITO", carrito);
                    }
                }
            }

            return RedirectToAction("Carrito");
        }

        public IActionResult Carrito()
        {
            var user = HttpContext.Session.GetString("USUARIO");
            if (user == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            else
            { 
                var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();
                return View(carrito);
            }

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
            }

            return PartialView("_Carrito", carrito);
        }
    }
}
