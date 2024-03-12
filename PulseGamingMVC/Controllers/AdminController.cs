using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using System.Drawing.Text;
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

        private ListasCrearJuego listasCrearJuego = new ListasCrearJuego();

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

        public IActionResult JuegosView()
        {
            List<Juego> juegos = this.repoGame.GetJuegos();
            return View(juegos);
        }

        public IActionResult GenerosEditoresView()
        {
            return View();
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

        public async Task<IActionResult> CreateJuego()
        {
            List<Genero> generos = await this.repoGame.GetGenerosAsync();
            List<Editor> editores = await this.repoGame.GetEditoresAsync();

            ViewData["GENEROS"] = generos;
            ViewData["EDITORES"] = editores;

            return View();
        }

        [HttpPost]
        public IActionResult CreateJuego(Juego juego)
        {
            this.repoGame.RegistrarJuego(juego.NombreJuego, juego.IdGenero, juego.ImagenJuego, juego.PrecioJuego, juego.Descripcion, juego.IdEditor);
            return RedirectToAction("JuegosView");
        }

        public IActionResult DeleteJuego(int idJuego)
        {
            this.repoGame.DeleteJuego(idJuego);
            return View();
        }

        public async Task<IActionResult> ModificarJuego(int idJuego)
        {
            Juego juego = this.repoGame.FindJuego(idJuego);
            List<Genero> generos = await this.repoGame.GetGenerosAsync();
            List<Editor> editores = await this.repoGame.GetEditoresAsync();

            ViewData["GENEROS"] = generos;
            ViewData["EDITORES"] = editores;
            return View(juego);
        }

        [HttpPost]
        public IActionResult ModificarJuego(Juego juego)
        {
            this.repoGame.ModificarJuego(juego.IdJuego, juego.NombreJuego, juego.IdGenero, juego.ImagenJuego, juego.PrecioJuego, juego.Descripcion, juego.IdEditor);
            return View();
        }
    }
}
