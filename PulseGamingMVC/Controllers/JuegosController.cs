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

        public IActionResult Details(int IdJuego)
        {
            Juego juegoDetalle = this.repo.FindJuego(IdJuego);
            return View(juegoDetalle);
        }

        [HttpPost]
        public IActionResult AñadirCarrito(int? IdJuego)
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
            return RedirectToAction("Details", juegoDetalle);
        }

        public IActionResult Carrito()
        {
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();
            return View(carrito);

        }

        public IActionResult Pedidos()
        {
            // Obtener el pedido de la sesión
            var pedido = HttpContext.Session.GetObject<Pedido>("PEDIDO");

            // Mostrar la vista de pedidos con el pedido guardado en la sesión
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
                Ciudad = ciudad, // Reemplaza con la ciudad real del usuario
                Pais = pais, // Reemplaza con el país real del usuario
                IDUsuario = 1, // Reemplaza con el ID real del usuario autenticado
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
            // 1. Obtener el carrito actual del usuario
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            // 2. Buscar el juego en el carrito por su ID
            var juegoEnCarrito = carrito.FirstOrDefault(item => item.IdJuego == idJuego);

            // 3. Incrementar la cantidad del juego si se encontró en el carrito
            if (juegoEnCarrito != null)
            {
                juegoEnCarrito.Cantidad++;
            }

            // 4. Actualizar el carrito en la sesión
            HttpContext.Session.SetObject("CARRITO", carrito);

            // 5. Devolver una vista parcial o un fragmento de HTML que representa la parte actualizada del carrito
            return PartialView("_Carrito", carrito);
        }

        [HttpPost]
        public IActionResult DecrementarCantidad(int idJuego)
        {
            // 1. Obtener el carrito actual del usuario
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            // 2. Buscar el juego en el carrito por su ID
            var juegoEnCarrito = carrito.FirstOrDefault(item => item.IdJuego == idJuego);

            // 3. Incrementar la cantidad del juego si se encontró en el carrito
            if (juegoEnCarrito != null)
            {
                juegoEnCarrito.Cantidad--;
            }

            // 4. Actualizar el carrito en la sesión
            HttpContext.Session.SetObject("CARRITO", carrito);

            // 5. Devolver una vista parcial o un fragmento de HTML que representa la parte actualizada del carrito
            return PartialView("_Carrito", carrito);
        }

        public IActionResult EliminarJuego(int idJuego)
        {
            var carrito = HttpContext.Session.GetObject<List<Carrito>>("CARRITO") ?? new List<Carrito>();

            // Buscar el juego en el carrito y eliminarlo
            var juegoAEliminar = carrito.FirstOrDefault(item => item.IdJuego == idJuego);
            if (juegoAEliminar != null)
            {
                carrito.Remove(juegoAEliminar);
                HttpContext.Session.SetObject("CARRITO", carrito);
                TempData["SuccessMessage"] = "Juego eliminado del carrito";
            }

            // Redirigir de vuelta a la vista del carrito
            return PartialView("_Carrito", carrito);
        }
    }
}
