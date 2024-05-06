using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PulseGamingMVC.Data;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PulseGamingMVC.Services
{
    public class ServicePulseGaming
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;
        private IHttpContextAccessor httpContextAccessor;
        private PulseGamingContext context;

        private XDocument document;

        public ServicePulseGaming(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PulseGamingContext context, HelperPathProvider helper)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiJuegos");
            string path = helper.MapPath("");
            this.document = XDocument.Load(path);
        }

        public async Task<string> GetTokenAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                LoginModel model = new LoginModel
                {
                    Email = email,
                    Password = password
                };
                string jsonData = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonData, Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await
                    client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
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

        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
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

        public async Task<List<Juego>> GetJuegosAsync()
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

        public async Task<Usuario> GetPerfilUsuarioAsync()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/usuarios/perfilusuario";
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }

        public async Task<List<Juego>> GetGrupoJuegosAsync(int posicion)
        {
            string request = "api/juegos/getpaginacionjuegos/" + posicion;
            List<Juego> data = await this.CallApiAsync<List<Juego>>(request);
            return data;
        }

        public async Task<List<Juego>> GetJuegosGenerosAsync(int idgenero)
        {
            string request = "api/juegos/getjuegosgeneros/" + idgenero;
            List<Juego> data = await this.CallApiAsync<List<Juego>>(request);
            return data;
        }

        public async Task<List<Juego>> GetJuegosPrecioDescAsync()
        {
            string request = "api/juegos/getjuegospreciodesc";
            List<Juego> data = await this.CallApiAsync<List<Juego>>(request);
            return data;
        }

        public async Task<List<Juego>> GetJuegosPrecioAsceAsync()
        {
            string request = "api/juegos/getjuegosprecioasce";
            List<Juego> data = await this.CallApiAsync<List<Juego>>(request);
            return data;
        }

        public async Task<int> GetNumeroJuegosAsync()
        {
            string request = "api/juegos/getnumerojuegos";
            int data = await this.CallApiAsync<int>(request);
            return data;
        }

        public async Task<List<DetallePedidoView>> GetProductosPedidoUsuarioAsync(int idusuario)
        {
            string request = "api/juegos/getproductospedidousuario/" + idusuario;
            List<DetallePedidoView> data = await this.CallApiAsync<List<DetallePedidoView>>(request);
            return data;
        }

        public async Task<int> GetMaxIdDetallePedidoAsync()
        {
            string request = "api/juegos/getmaxiddetallepedido";
            int data = await this.CallApiAsync<int>(request);
            return data;
        }

        public async Task<int> GetMaxIdPedidoAsync()
        {
            string request = "api/juegos/getmaxidpedido";
            int data = await this.CallApiAsync<int>(request);
            return data;
        }

        public async Task InsertPedidoAsync(int idusuario, List<Juego> carrito)
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(z => z.Type == "TOKEN").Value;
            using (HttpClient client = new HttpClient())
            {
                string request = "api/juegos/insertpedido";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                var total = 0.0m;
                foreach (Juego juego in carrito)
                {
                    total = juego.PrecioJuego + total;
                }
                Pedido pedido = new Pedido
                {
                    IDPedido = await GetMaxIdPedidoAsync(),
                    IDUsuario = idusuario,
                    FechaPedido = DateTime.Now,
                    Total = total
                };
                var data = new
                {
                    pedido,
                    carrito,
                };
                string jsonData = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Insertado Pedido");
                }
                else
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync());
                }
            }
        }

        public async Task InsertJuegoAsync(int id, string nombre, int idgenero, string imagen, decimal precio, string descripcion, int ideditor)
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(z => z.Type == "TOKEN").Value;
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/insertjuego";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
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
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(z => z.Type == "TOKEN").Value;
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/updatejuego";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
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
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(z => z.Type == "TOKEN").Value;
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/deletejuego/" + idJuego;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.DeleteAsync(request);

            }
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            string request = "api/admin/getgeneros";
            List<Genero> data = await this.CallApiAsync<List<Genero>>(request);
            return data;
        }

        public async Task<Genero> GetGeneroAsync(int idgenero)
        {
            string request = "api/admin/getgenero" + idgenero;
            Genero data = await this.CallApiAsync<Genero>(request);
            return data;
        }

        public async Task InsertGeneroAsync(int id, string nombre)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/insertgenero";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Genero genero = new Genero();
                genero.IdGenero = id;
                genero.NombreGenero = nombre;
                string json = JsonConvert.SerializeObject(genero);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdateGeneroAsync(int id, string nombre)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/updategenero";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Genero genero = new Genero();
                genero.IdGenero = id;
                genero.NombreGenero = nombre;
                string json = JsonConvert.SerializeObject(genero);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeleteGeneroAsync(int idgenero)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/deletegenero/" + idgenero;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.DeleteAsync(request);

            }
        }

        public async Task<List<Editor>> GetEditoresAsync()
        {
            string request = "api/admin/geteditores";
            List<Editor> data = await this.CallApiAsync<List<Editor>>(request);
            return data;
        }

        public async Task<Editor> GetEditorAsync(int ideditor)
        {
            string request = "api/admin/geteditor" + ideditor;
            Editor data = await this.CallApiAsync<Editor>(request);
            return data;
        }

        public async Task InsertEditorAsync(int id, string nombre)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/inserteditor";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Editor editor = new Editor();
                editor.IDEditor = id;
                editor.NombreEditor = nombre;
                string json = JsonConvert.SerializeObject(editor);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdateEditorAsync(int id, string nombre)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/updateeditor";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Editor editor = new Editor();
                editor.IDEditor = id;
                editor.NombreEditor = nombre;
                string json = JsonConvert.SerializeObject(editor);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeleteEditorAsync(int ideditor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/deleteeditor/" + ideditor;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.DeleteAsync(request);

            }
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            string request = "api/admin/getusuarios";
            List<Usuario> data = await this.CallApiAsync<List<Usuario>>(request);
            return data;
        }

        public async Task<Usuario> GetUsuarioAsync(int idusuario)
        {
            string request = "api/admin/getusuario" + idusuario;
            Usuario data = await this.CallApiAsync<Usuario>(request);
            return data;
        }

        public async Task InsertUsuarioAsync(int id, string password, string nombre, string apellidos, string email, int telefono, int IDRole)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/insertusuario";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Usuario usuario = new Usuario();
                usuario.IdUsuario = id;
                usuario.Password = password;
                usuario.Nombre = nombre;
                usuario.Apellidos = apellidos;
                usuario.Email = email;
                usuario.Telefono = telefono;
                usuario.IDRole = IDRole;
                string json = JsonConvert.SerializeObject(usuario);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdateUsuarioAsync(int id, string password, string nombre, string apellidos, string email, int telefono, int IDRole)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/updateusuario";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                Usuario usuario = new Usuario();
                usuario.IdUsuario = id;
                usuario.Password = password;
                usuario.Nombre = nombre;
                usuario.Apellidos = apellidos;
                usuario.Email = email;
                usuario.Telefono = telefono;
                usuario.IDRole = IDRole;
                string json = JsonConvert.SerializeObject(usuario);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeleteUsuarioAsync(int idusuario)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/admin/deleteusuario/" + idusuario;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.DeleteAsync(request);

            }
        }
    }
}
