using Newtonsoft.Json;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using StackExchange.Redis;
using System.Security.Claims;

namespace PulseGamingMVC.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;
        private IHttpContextAccessor httpContextAccessor;
        public ServiceCacheRedis(IHttpContextAccessor httpContextAccessor)
        {
            this.database = HelperCacheMultiplexer.Connection.GetDatabase();
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddJuegoFavoritoAsync(Juego juego)
        {
            if (this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var idUsuario = int.Parse(this.httpContextAccessor.HttpContext.User.FindFirst("IdUsuario").Value);

                string jsonJuegos = await this.database.StringGetAsync("favoritos" + "-" + idUsuario);
                List<Juego> juegosList;
                if (jsonJuegos == null)
                {
                    juegosList = new List<Juego>();
                }
                else
                {
                    juegosList = JsonConvert.DeserializeObject<List<Juego>>(jsonJuegos);
                }

                juegosList.Add(juego);

                jsonJuegos = JsonConvert.SerializeObject(juegosList);

                await this.database.StringSetAsync("favoritos" + "-" + idUsuario, jsonJuegos);
            }
        }

        public async Task<List<Juego>> GetJuegosFavoritosAsync()
        {
            int idusuario = int.Parse(this.httpContextAccessor.HttpContext.User.FindFirstValue("IdUsuario"));

            string jsonJuegos = await this.database.StringGetAsync("favoritos" + "-" + idusuario);
            if (jsonJuegos == null)
            {
                return null;
            }
            else
            {
                List<Juego> favoritos = JsonConvert.DeserializeObject<List<Juego>>(jsonJuegos);
                return favoritos;
            }
        }

        public async Task DeleteJuegoFavoritoAsync(int idjuego)
        {
            int idusuario = int.Parse(this.httpContextAccessor.HttpContext.User
                    .FindFirstValue("IdUsuario"));
            List<Juego> favoritos = await this.GetJuegosFavoritosAsync();
            if (favoritos != null)
            {
                Juego juego = favoritos.FirstOrDefault(z => z.IdJuego == idjuego);

                favoritos.Remove(juego);

                if (favoritos.Count == 0)
                {

                    await this.database.KeyDeleteAsync("favoritos" + "-" + idusuario);
                }
                else
                {

                    string jsonjuegos = JsonConvert.SerializeObject(favoritos);

                    await this.database.StringSetAsync("favoritos" + "-" + idusuario, jsonjuegos, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
