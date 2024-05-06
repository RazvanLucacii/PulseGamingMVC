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

        public async Task AddJuegoFavorito(Juego juego)
        {
            if (this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                int idUsuario = int.Parse(this.httpContextAccessor.HttpContext.User.FindFirstValue("IdUsuario"));

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

        public List<Juego> GetJuegosFavoritos()
        {
            string jsonJuegos = this.database.StringGet("favoritos");
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

        public void DeleteJuegoFavorito(int idjuego)
        {
            List<Juego> favoritos = this.GetJuegosFavoritos();
            if (favoritos != null)
            {
                Juego juego = favoritos.FirstOrDefault(z => z.IdJuego == idjuego);

                favoritos.Remove(juego);

                if (favoritos.Count == 0)
                {

                    this.database.KeyDelete("favoritos");
                }
                else
                {

                    string jsonjuegos = JsonConvert.SerializeObject(favoritos);

                    this.database.StringSet("favoritos", jsonjuegos, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
