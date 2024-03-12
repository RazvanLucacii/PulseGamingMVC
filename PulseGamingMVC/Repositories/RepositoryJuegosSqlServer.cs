using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Models;
using System.Data;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_TODOS_JUEGOS
//as
//	select * from Juego
//go

//create procedure SP_DETALLES_JUEGO
//(@IDJuego int)
//as
//	select * from Juego where IDJuego = @IDJuego
//go

//create procedure SP_INSERT_JUEGO
//(@NombreJuego nvarchar(100), @IDGenero int, @Imagen nvarchar(100), @Precio float, @Descripcion nvarchar(MAX), @IDEditor int)
//as
//	DECLARE @NEXTID INT
//	SELECT @NEXTID = MAX(IDJuego) +1 FROM Juego
//	INSERT INTO Juego VALUES (@NEXTID, @NombreJuego, @IDGenero, @Imagen, @Precio, @Descripcion, @IDEditor)
//go

//create procedure SP_DELETE_JUEGO
//(@IDJuego int)
//as
//	delete from juego
//	where IDJuego = @IDJuego
//go

//alter procedure SP_MODIFICAR_JUEGO
//(@IDJuego int, @NombreJuego nvarchar(100), @IDGenero int, @ImagenJuego nvarchar(100), @PrecioJuego float, @Descripcion nvarchar(MAX), @IDEditor int)
//as
//	update Juego set NombreJuego=@NombreJuego, IDGenero = @IDGenero, ImagenJuego = @ImagenJuego, PrecioJuego = @PrecioJuego, DescripcionJuego = @Descripcion, IDEditor = @IDEditor where IDJuego = @IDJuego
//go


#endregion

namespace PulseGamingMVC.Repositories
{
    public class RepositoryJuegosSqlServer : IRepositoryJuegos
    {
        private PulseGamingContext context;

        public RepositoryJuegosSqlServer(PulseGamingContext context)
        {
            this.context = context;
        }
        public List<Juego> GetJuegos()
        {
            string sql = "SP_TODOS_JUEGOS";
            var consulta = this.context.Juegos.FromSqlRaw(sql);
            return consulta.ToList();
        }

        public Juego FindJuego(int IdJuego)
        {
            string sql = "SP_DETALLES_JUEGO @IDJuego";
            SqlParameter pamId = new SqlParameter("@IDJuego", IdJuego);
            var consulta = this.context.Juegos.FromSqlRaw(sql, pamId);
            Juego juego = consulta.AsEnumerable().FirstOrDefault();
            return juego;
        }

        public void RegistrarJuego(string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor)
        {
            string sql = "SP_INSERT_JUEGO @NombreJuego, @IDGenero, @Imagen, @Precio, @Descripcion, @IDEditor";
            SqlParameter pamNombre = new SqlParameter("NombreJuego", nombre);
            SqlParameter pamIDGenero = new SqlParameter("@IDGenero", idGenero);
            SqlParameter pamImagen = new SqlParameter("@Imagen", imagen);
            SqlParameter pamPrecio = new SqlParameter("@Precio", precio);
            SqlParameter pamDescripcion = new SqlParameter("@Descripcion", descripcion);
            SqlParameter pamIDEditor = new SqlParameter("@IDEditor", idEditor);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre, pamIDGenero, pamImagen, pamPrecio, pamDescripcion, pamIDEditor);

        }

        public void ModificarJuego(int idJuego, string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor)
        {
            string sql = "SP_MODIFICAR_JUEGO @IDJuego, @NombreJuego, @IDGenero, @ImagenJuego, @PrecioJuego, @Descripcion, @IDEditor";
            SqlParameter pamIdJuego = new SqlParameter("IDJuego", idJuego);
            SqlParameter pamNombre = new SqlParameter("NombreJuego", nombre);
            SqlParameter pamIDGenero = new SqlParameter("IDGenero", idGenero);
            SqlParameter pamImagen = new SqlParameter("ImagenJuego", imagen);
            SqlParameter pamPrecio = new SqlParameter("PrecioJuego", precio);
            SqlParameter pamDescripcion = new SqlParameter("Descripcion", descripcion);
            SqlParameter pamIDEditor = new SqlParameter("IDEditor", idEditor);
            this.context.Database.ExecuteSqlRaw(sql, pamIdJuego, pamNombre, pamIDGenero, pamImagen, pamPrecio, pamDescripcion, pamIDEditor);
        }

        public void DeleteJuego(int idjuego)
        {
            string sql = "SP_DELETE_JUEGO @IDJuego";
            SqlParameter pamid = new SqlParameter("@IDJuego", idjuego);
            this.context.Database.ExecuteSqlRaw(sql, pamid);
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<List<Editor>> GetEditoresAsync()
        {
            return await this.context.Editores.ToListAsync();
        }
    }
}
