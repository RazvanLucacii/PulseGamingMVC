using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MvcCoreAzureStorage.Services;
using PulseGamingMVC.Data;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using PulseGamingMVC.Services;
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
        private ServiceStorageBlobs storageBlobs;
        private IRepositoryJuegos repoGame;
        private IRepositoryUsuarios repoUsu;
        private ServicePulseGaming service;

        private ListasCrearJuego listasCrearJuego = new ListasCrearJuego();

        public AdminController(IRepositoryJuegos repoGame, IRepositoryUsuarios repoUsu, ServiceStorageBlobs storageBlobs, ServicePulseGaming service)
        {
            this.repoGame = repoGame;
            this.repoUsu = repoUsu;
            this.storageBlobs = storageBlobs;
            this.service = service;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        //USUARIOS
        public async Task<IActionResult> UsuariosView()
        {
            List<Usuario> usuarios = await this.service.GetUsuariosAsync();
            return View(usuarios);
        }


        public IActionResult CreateUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(Usuario usu)
        {
            await this.service.InsertUsuarioAsync(usu.IdUsuario, usu.Password, usu.Nombre, usu.Apellidos, usu.Email, usu.Telefono, usu.IDRole);
            return RedirectToAction("UsuariosView");
        }

        public async Task<IActionResult> ModificarUsuario(int idUsuario)
        {
            Usuario usuario = await this.service.GetUsuarioAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult ModificarUsuario(Usuario usu)
        {
            this.service.UpdateUsuarioAsync(usu.IdUsuario, usu.Password, usu.Nombre, usu.Apellidos, usu.Email, usu.Telefono, usu.IDRole);
            return RedirectToAction("UsuariosView");
        }

        public async Task<IActionResult> DeleteUsuario(int idUsuario)
        {
            await this.service.DeleteUsuarioAsync(idUsuario);
            return RedirectToAction("UsuariosView");
        }

        //JUEGOS
        public async Task<IActionResult> JuegosView()
        {
            List<Juego> juegos = await this.service.GetJuegosAsync();
            return View(juegos);
        }

        public async Task<IActionResult> CreateJuego()
        {
            List<Genero> generos = await this.service.GetGenerosAsync();
            List<Editor> editores = await this.service.GetEditoresAsync();

            ViewData["GENEROS"] = generos;
            ViewData["EDITORES"] = editores;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJuego(Juego juego, IFormFile imagen)
        {
            await this.storageBlobs.UploadBlobAsync("imgs", imagen.FileName, imagen.OpenReadStream());
            juego.ImagenJuego = await this.storageBlobs.GetBlobUrlAsync("imgs", imagen.FileName);
            await this.service.InsertJuegoAsync(juego.IdJuego, juego.NombreJuego, juego.IDGenero, juego.ImagenJuego, juego.PrecioJuego, juego.Descripcion, juego.IdEditor);
            return RedirectToAction("JuegosView");
        }

        public async Task<IActionResult> ModificarJuego(int idJuego)
        {
            Juego juego = await this.service.GetJuegoAsync(idJuego);
            List<Genero> generos = await this.service.GetGenerosAsync();
            List<Editor> editores = await this.service.GetEditoresAsync();

            ViewData["GENEROS"] = generos;
            ViewData["EDITORES"] = editores;
            return View(juego);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarJuego(Juego juego)
        {
            await this.service.UpdateJuegoAsync(juego.IdJuego, juego.NombreJuego, juego.IDGenero, juego.ImagenJuego, juego.PrecioJuego, juego.Descripcion, juego.IdEditor);
            return RedirectToAction("JuegosView");
        }

        public async Task<IActionResult> DeleteJuego(int idJuego)
        {
            await this.service.DeleteJuegoAsync(idJuego);
            return RedirectToAction("JuegosView");
        }

        //Generos
        public async Task<IActionResult> GenerosView()
        {
            List<Genero> generos = await this.service.GetGenerosAsync();
            return View(generos);
        }

        public IActionResult CreateGenero()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenero(Genero genero)
        {
            await this.service.InsertGeneroAsync(genero.IdGenero, genero.NombreGenero);
            return RedirectToAction("GenerosView");
        }

        public async Task<IActionResult> ModificarGenero(int idGenero)
        {
            Genero genero = await this.service.GetGeneroAsync(idGenero);
            return View(genero);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarGenero(Genero genero)
        {
            await this.service.UpdateGeneroAsync(genero.IdGenero, genero.NombreGenero);
            return RedirectToAction("GenerosView");
        }

        public async Task<IActionResult> DeleteGenero(int idGenero)
        {
            await this.service.DeleteGeneroAsync(idGenero);
            return RedirectToAction("GenerosView");
        }

        //Editores
        public async Task<IActionResult> EditoresView()
        {
            List<Editor> editores = await this.service.GetEditoresAsync();
            return View(editores);
        }

        public IActionResult CreateEditor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditor(Editor editor)
        {
            await this.service.InsertEditorAsync(editor.IDEditor, editor.NombreEditor);
            return RedirectToAction("EditoresView");
        }

        public async Task<IActionResult> ModificarEditor(int idEditor)
        {
            Editor editor = await this.service.GetEditorAsync(idEditor);
            return View(editor);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarEditor(Editor editor)
        {
            await this.service.UpdateEditorAsync(editor.IDEditor, editor.NombreEditor);
            return RedirectToAction("EditoresView");
        }

        public async Task<IActionResult> DeleteEditor(int idEditor)
        {
            await this.service.DeleteEditorAsync(idEditor);
            return RedirectToAction("EditoresView");
        }
    }
}
