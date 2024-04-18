using Newtonsoft.Json;
using PulseGamingMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PulseGamingMVC.Services
{
    public class ServicePulseGaming
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;


        public ServicePulseGaming(IConfiguration configuration)
        {
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiJuegos");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Juego>> GetJuegpsAsync()
        {
            string request = "api/juegos";
            List<Juego> data = await this.CallApiAsync<List<Juego>>(request);
            return data;
        }

        public async Task<Juego> GetJuegoAsync(int idJuego)
        {
            string request = "api/juegos/" + idJuego;
            Juego data = await this.CallApiAsync<Juego>(request);
            return data;
        }

        public async Task InsertJuegoAsync(int id, string nombre, int idgenero, string imagen, decimal precio, string descripcion, int ideditor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/juegos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Juego juego = new Juego();
                juego.IdJuego = id;
                juego.NombreJuego = nombre;
                juego.IDGenero = idgenero;
                juego.ImagenJuego = imagen;
                juego.PrecioJuego = precio;
                juego.Descripcion = descripcion;
                juego.IdEditor = ideditor;
                string json = JsonConvert.SerializeObject(juego);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdateJuegoAsync(int id, string nombre, int idgenero, string imagen, decimal precio, string descripcion, int ideditor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/juegos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Juego juego = new Juego();
                juego.IdJuego = id;
                juego.NombreJuego = nombre;
                juego.IDGenero = idgenero;
                juego.ImagenJuego = imagen;
                juego.PrecioJuego = precio;
                juego.Descripcion = descripcion;
                juego.IdEditor = ideditor;
                string json = JsonConvert.SerializeObject(juego);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeleteJuegoAsync(int idJuego)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/juegos/" + idJuego;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.DeleteAsync(request);

            }
        }
    }
}
