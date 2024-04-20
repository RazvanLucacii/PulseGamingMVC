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
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Email, ClaimTypes.Role);

                identity.AddClaim
                    (new Claim(ClaimTypes.Email, model.Email));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, model.Password));
                identity.AddClaim
                    (new Claim("TOKEN", token));
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
