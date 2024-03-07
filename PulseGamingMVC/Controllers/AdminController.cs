using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_DETALLES_USUARIO
//(@IDUsuario int)
//as
//	select * from Usuarios where IDUsuario = @IDUsuario 
//go

#endregion

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

        public IActionResult CreateUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUsuario(string nombre, string apellidos, string email, string password, int telefono, int IDRole)
        {
            this.repoUsu.RegisterUser(nombre, apellidos, email, password, telefono, IDRole);
            ViewData["MENSAJE"] = "Usuario registrado correctamente.";
            return View();
        }

        public IActionResult ModificarUsuario(int idUsuario)
        {
            Usuario usuario = this.repoUsu.FindUsuarioById(idUsuario);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult ModificarUsuario(int idUsuario, string nombre, string apellidos, string email, string password, int telefono, int IDRole)
        {
            this.repoUsu.ModificarUsuario(idUsuario, nombre, apellidos, email, password, telefono, IDRole);
            return RedirectToAction("UsuariosView");
        }

        public IActionResult DeleteUsuario(int idUsuario)
        {
            this.repoUsu.DeleteUsuario(idUsuario);
            return RedirectToAction("UsuariosView");
        }

        public IActionResult CreateJuego()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateJuego(Juego juego)
        {
            this.repoGame.RegistrarJuego(juego.NombreJuego, juego.IdGenero, juego.ImagenJuego, juego.PrecioJuego, juego.Descripcion);
            return RedirectToAction("JuegosGeneros");
        }
    }
}
