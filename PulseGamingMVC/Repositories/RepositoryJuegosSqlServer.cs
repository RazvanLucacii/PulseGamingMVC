using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Models;
using System.Diagnostics.Metrics;

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

//create PROCEDURE SP_INSERT_JUEGO
//(@NombreJuego NVARCHAR(100), @IDGenero INT, @Imagen NVARCHAR(255), @Precio FLOAT, @Descripcion NVARCHAR(MAX))
//as
//    IF NOT EXISTS (SELECT 1 FROM Genero WHERE IDGenero = @IDGenero)
//    begin
//        SELECT 'El ID del género no existe' AS Mensaje
//        RETURN
//    end
	
//	DECLARE @NEXTID INT
//	SELECT @NEXTID = MAX(IDJuego) +1 FROM Juego
//    INSERT INTO Juego VALUES (@NEXTID, @NombreJuego, @IDGenero, @Imagen, @Precio, @Descripcion)
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

        public void RegistrarJuego(string nombre, int idGenero, string imagen, double precio, string descripcion)
        {
            string sql = "SP_INSERT_JUEGO @NombreJuego, @IDGenero, @Imagen, @Precio, @Descripcion";
            SqlParameter pamNombre = new SqlParameter("NombreJuego", nombre);
            SqlParameter pamIDGenero = new SqlParameter("@IDGenero", idGenero);
            SqlParameter pamImagen = new SqlParameter("@Imagen", imagen);
            SqlParameter pamPrecio = new SqlParameter("@Precio", precio);
            SqlParameter pamDescripcion = new SqlParameter("@Descripcion", descripcion);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre, pamIDGenero, pamImagen, pamPrecio, pamDescripcion);

        }
    }
}
