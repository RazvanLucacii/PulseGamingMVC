﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PulseGamingMVC.Data;
using PulseGamingMVC.Extensions;
using PulseGamingMVC.Filters;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using PulseGamingMVC.Services;
using System;
using System.Security.Claims;

namespace PulseGamingMVC.Controllers
{
    public class JuegosController : Controller
    {
        private IRepositoryJuegos repo;
        private IMemoryCache memoryCache;
        private ServicePulseGaming service;
        private ServiceCacheRedis serviceCache;

        public JuegosController(IRepositoryJuegos repo, IMemoryCache memoryCache, ServicePulseGaming service, ServiceCacheRedis serviceCache)
        {
            this.service = service;
            this.repo = repo;
            this.memoryCache = memoryCache;
            this.serviceCache = serviceCache;
        }

        public IActionResult Inicio()
        {
            return View();
        }

        public async Task<IActionResult> JuegosFavoritos()
        {
            List<Juego> juegos = await this.serviceCache.GetJuegosFavoritosAsync();
            return View(juegos);
        }

        public async Task<IActionResult> Games(string precio, string search, int? posicion, int? idfavorito)
        {

            ViewData["PRECIO"] = precio;

            if (posicion == null)
            {
                posicion = 1;
            }
            int numeroRegistros = await this.service.GetNumeroJuegosAsync();
            ViewData["REGISTROS"] = numeroRegistros;
            ViewData["POSICION"] = posicion;

            if (precio == "desc")
            {
                var juegosDesc = await this.service.GetJuegosPrecioDescAsync();
                return View(juegosDesc);
            }
            else if(precio == "asc")
            {
                var juegosasc = await this.service.GetJuegosPrecioAsceAsync();
                return View(juegosasc);
            }
            else
            {
                List<Juego> juegos = await this.service.GetGrupoJuegosAsync(posicion.Value);
                return View(juegos);
            }

        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> AñadirFavorito(int idjuego)
        {
            Juego juego = await this.service.GetJuegoAsync(idjuego);
            await this.serviceCache.AddJuegoFavoritoAsync(juego);
            return RedirectToAction("Games");
        }

        public async Task<IActionResult> DeleteFavorito(int idjuego)
        {
            await this.serviceCache.DeleteJuegoFavoritoAsync(idjuego);
            return RedirectToAction("JuegosFavoritos");
        }

        public async Task<IActionResult> Details(int IdJuego)
        {
            Juego juegoDetalle = await this.service.GetJuegoAsync(IdJuego);
            return View(juegoDetalle);
        }

        public async Task<IActionResult> ListarJuegosCategorias(int idgenero)
        {
            List<Juego> juegosCategorias = await this.service.GetJuegosGenerosAsync(idgenero);
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

        [AuthorizeUsuarios]
        public async Task<IActionResult> RealizarCompra()
        {
            var idusuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);

            // Obtenemos la lista de productos en la cesta desde la sesión
            List<int> cesta = HttpContext.Session.GetObject<List<int>>("CARRITO");

            // Obtenemos los detalles de los productos en la cesta desde la base de datos
            List<Juego> productosEnCarrito = await this.repo.GetProductosEnCarritoAsync(cesta);

            // Creamos el pedido
            await this.service.InsertPedidoAsync(idusuario, productosEnCarrito);

            // Limpiamos la cesta después de crear el pedido
            HttpContext.Session.Remove("CARRITO");

            // Redirigimos a alguna página de confirmación o cualquier otra acción que necesites
            return RedirectToAction("Games");
        }

        public async Task<IActionResult> PedidosUsuario()
        {
            var idUsuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
            List<DetallePedidoView> pedidosUsuarios = await this.service.GetProductosPedidoUsuarioAsync(idUsuario);
            return View(pedidosUsuarios);
        }
    }
}
