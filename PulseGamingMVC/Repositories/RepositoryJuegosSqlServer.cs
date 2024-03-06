using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Models;

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

    }
}
