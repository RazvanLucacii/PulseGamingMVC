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

        //USUARIOS
        public IActionResult UsuariosView()
        {
            List<Usuario> usuarios = this.repoUsu.GetUsuarios();
            return View(usuarios);
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

        //JUEGOS
        public IActionResult JuegosView()
        {
            List<Juego> juegos = this.repoGame.GetJuegos();
            return View(juegos);
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
            return RedirectToAction("JuegosView", "Admin");
        }

        public IActionResult DeleteJuego(int idJuego)
        {
            this.repoGame.DeleteJuego(idJuego);
            return View();
        }

        //Generos
        public async Task<IActionResult> GenerosView()
        {
            List<Genero> generos = await this.repoGame.GetGenerosAsync();
            return View(generos);
        }

        public IActionResult CreateGenero()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGenero(Genero genero)
        {
            this.repoGame.CrearGenero(genero.NombreGenero);
            return RedirectToAction("GenerosView", "Admin");
        }

        public async Task<IActionResult> ModificarGenero(int idGenero)
        {
            Genero genero = await this.repoGame.FindGeneroAsync(idGenero);
            return View(genero);
        }

        [HttpPost]
        public IActionResult ModificarGenero(Genero genero)
        {
            this.repoGame.ModificarGenero(genero.IdGenero, genero.NombreGenero);
            return RedirectToAction("GenerosView", "Admin");
        }

        public IActionResult DeleteGenero(int idGenero)
        {
            this.repoGame.DeleteGenero(idGenero);
            return View();
        }

        //Editores
        public async Task<IActionResult> EditoresView()
        {
            List<Editor> editores = await this.repoGame.GetEditoresAsync();
            return View(editores);
        }

        public IActionResult CreateEditor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEditor(Editor editor)
        {
            this.repoGame.CrearEditor(editor.NombreEditor);
            return RedirectToAction("EditoresView", "Admin");
        }

        public async Task<IActionResult> ModificarEditor(int idEditor)
        {
            Editor editor = await this.repoGame.FindEditorAsync(idEditor);
            return View(editor);
        }

        [HttpPost]
        public IActionResult ModificarGenero(Editor editor)
        {
            this.repoGame.ModificarEditor(editor.IDEditor, editor.NombreEditor);
            return RedirectToAction("EditoresView", "Admin");
        }

        public IActionResult DeleteEditor(int idEditor)
        {
            this.repoGame.DeleteEditor(idEditor);
            return View();
        }
    }
}
