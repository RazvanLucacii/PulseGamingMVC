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
                    List<Juego> juegosFavoritos;
                    if (this.memoryCache.Get("FAVORITOS") == null)
                    {
                        juegosFavoritos = new List<Juego>();
                    }
                    else
                    {
                        juegosFavoritos =
                            this.memoryCache.Get<List<Juego>>("FAVORITOS");
                    }
                    Juego juego =
                        this.repo.FindJuego(idfavorito.Value);
                    juegosFavoritos.Add(juego);
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

        public async Task<IActionResult> Carrito()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                List<Juego> juegos = await this.repo.GetJuegosSessionAsync(carrito);
                return View(juegos);
            }
            return View();
        }

        public IActionResult AñadirCarrito(int? idJuego)
        {
            if (idJuego != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetString("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idJuego.Value);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            return RedirectToAction("Games");
        }

        [HttpPost]
        public IActionResult AñadirAlCarritoDesdeLista(int? idJuego)
        {

            if (idJuego != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetString("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idJuego.Value);
                HttpContext.Session.SetObject("CARRITO", carrito); 
            }

            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> EliminarJuegoCesta(int? idJuego)
        {
            if (idJuego != null)
            {
                List<int> carrito =
                    HttpContext.Session.GetObject<List<int>>("CARRITO");
                carrito.Remove(idJuego.Value);
                if (carrito.Count() == 0)
                {
                    HttpContext.Session.Remove("CARRITO");
                }
                else
                {
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
            }
            return RedirectToAction("Carrito");
        }

        private int ObtenerIdUsuario()
        {
            // Verificamos si el usuario está presente en la sesión
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");

            // Si el usuario está presente en la sesión, devolvemos su ID
            // Si no está presente o es nulo, devolvemos algún valor predeterminado o manejamos la situación según sea necesario
            return usuario != null ? usuario.IdUsuario : 0;
        }

        public async Task<IActionResult> RealizarCompra()
        {
            int idUsuario = ObtenerIdUsuario();

            // Obtenemos la lista de productos en la cesta desde la sesión
            List<int> cesta = HttpContext.Session.GetObject<List<int>>("CARRITO");

            // Obtenemos los detalles de los productos en la cesta desde la base de datos
            List<Juego> productosEnCarrito = await this.repo.GetProductosEnCarritoAsync(cesta);

            // Creamos el pedido
            await this.repo.CreatePedidoAsync(idUsuario, productosEnCarrito);

            // Limpiamos la cesta después de crear el pedido
            HttpContext.Session.Remove("CARRITO");

            // Redirigimos a alguna página de confirmación o cualquier otra acción que necesites
            return RedirectToAction("Games");
        }

        public async Task<IActionResult> PedidosUsuario()
        {
            int idUsuario = ObtenerIdUsuario();
            List<DetallePedidoView> pedidosUsuarios = await this.repo.GetProductosPedidoUsuarioAsync(idUsuario);
            return View(pedidosUsuarios);
        }
    }
}
