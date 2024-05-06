using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;
using PulseGamingMVC.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http.HttpResults;
using PulseGamingMVC.Services;
using PulseGamingMVC.Filters;


namespace PulseGamingMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private ServicePulseGaming service;
        private IRepositoryUsuarios repo;

        public UsuariosController(ServicePulseGaming service, IRepositoryUsuarios repo)
        {
            this.service = service;
            this.repo = repo;
        }

        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(Usuario usu)
        {
            await this.service.InsertUsuarioAsync(usu.IdUsuario, usu.Password, usu.Nombre, usu.Apellidos, usu.Email, usu.Telefono, usu.IDRole);
            return RedirectToAction("Perfil");
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            Usuario usuario = await this.service.GetPerfilUsuarioAsync();
            return View(usuario);
        }
    }
}
