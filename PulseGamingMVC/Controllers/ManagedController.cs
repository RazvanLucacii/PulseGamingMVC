using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Services;
using System.Security.Claims;
using PulseGamingMVC.Models;

namespace PulseGamingMVC.Controllers
{
    public class ManagedController : Controller
    {
        private ServicePulseGaming service;

        public ManagedController(ServicePulseGaming service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string token = await this.service
                .GetTokenAsync(model.Email, model.Password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Email/Password incorrectos";
                return View();
            }
            else
            {
                Usuario? usuario = (await service.GetUsuariosAsync())
                    .Where(user => user.Email.Equals(model.Email)
                    && user.Password.Equals(model.Password))
                    .FirstOrDefault();
                    
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Email, "Password");
                identity.AddClaim
                    (new Claim(ClaimTypes.Email, model.Email));
                identity.AddClaim
                    (new Claim("Password", model.Password));
                identity.AddClaim
                    (new Claim("TOKEN", token));
                identity.AddClaim(new Claim("IdUsuario", usuario!.IdUsuario.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, usuario.IDRole.ToString()));
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Inicio", "Juegos");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Inicio", "Juegos");
        }
    }
}
