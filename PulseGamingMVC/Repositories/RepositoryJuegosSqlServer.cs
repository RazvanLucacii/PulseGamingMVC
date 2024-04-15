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
//	update Juego set NombreJuego=@NombreJuego, IDGenero = @IDGenero, ImagenJuego = @ImagenJuego, PrecioJuego = @PrecioJuego, DescripcionJuego = @Descripcion, IDEditor = @IDEditor
//	where IDJuego = @IDJuego
//go

//create procedure SP_CREATE_GENERO
//(@NombreGenero nvarchar(100))
//as
//	DECLARE @NEXTID int
//	select @NEXTID = Max(IDGenero) + 1 from Genero
//	insert into Genero values(@NEXTID, @NombreGenero)
//go

//create procedure SP_CREATE_EDITOR
//(@NombreEditor nvarchar(100))
//as
//	DECLARE @NEXTID int
//	select @NEXTID = Max(IDEditor) + 1 from Editor
//	insert into Editor values(@NEXTID, @NombreEditor)
//go

//create procedure SP_DELETE_GENERO
//(@IDGenero int)
//as
//	delete from Genero
//	where IDGenero = @IDGenero
//go

//create procedure SP_MODIFICAR_GENERO
//(@IDGenero int, @NombreGenero nvarchar(100))
//as
//	update Genero set NombreGenero=@NombreGenero
//	where IDGenero = @IDGenero
//go

//create procedure SP_DELETE_EDITOR
//(@IDEditor int)
//as
//	delete from Editor
//	where IDEditor = @IDEditor
//go

//create procedure SP_MODIFICAR_EDITOR
//(@IDEditor int, @NombreEditor nvarchar(100))
//as
//	update Editor set NombreEditor=@NombreEditor
//	where IDEditor = @IDEditor
//go

//create procedure SP_FILTRAR_JUEGOS_CATEGORIAS
//(@idgenero int)
//as
//	select * from Juego
//	where Juego.IDGenero=@idgenero
//go

//create view V_GRUPO_JUEGOS
//as
//	select cast(
//    ROW_NUMBER() OVER (ORDER BY NombreJuego) as int) AS POSICION
//    , IDJuego, NombreJuego, IDGenero, ImagenJuego, PrecioJuego, DescripcionJuego, IDEditor from Juego
//go

//create procedure SP_GRUPO_JUEGOS
//(@posicion int)
//as
//	select IDJuego, NombreJuego, IDGenero, ImagenJuego, PrecioJuego, DescripcionJuego, IDEditor
//	from V_GRUPO_JUEGOS
//	where posicion >= @posicion and posicion < (@posicion + 4)
//go

//create VIEW V_DETALLES_PEDIDO AS
//SELECT 
//    dp.IDDetallePedido,
//    dp.IDPedido,
//    dp.IDJuego,
//    dp.Cantidad,
//    dp.PrecioUnitario,
//    j.NombreJuego AS NOMBRE_JUEGO,
//    dp.Cantidad * dp.PrecioUnitario AS TOTAL_DETALLE,
//    ped.Total AS TOTAL_PEDIDO,
//    ped.IDUsuario
//FROM 
//    DetallesPedido dp
//JOIN 
//    Juego j ON dp.IDJuego = j.IDJuego
//JOIN 
//    Pedidos ped ON dp.IDPedido = ped.IDPedido;

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

        public List<Juego> GetJuegosPrecioAsce()
        {
            return this.context.Juegos.OrderBy(z => z.PrecioJuego).ToList();
        }

        public List<Juego> GetJuegosPrecioDesc()
        {
            return this.context.Juegos.OrderByDescending(z => z.PrecioJuego).ToList();
        }

        public List<Juego> GetJuegosGeneros(int idgenero)
        {
            string sql = "SP_FILTRAR_JUEGOS_CATEGORIAS @idgenero";
            SqlParameter pamID = new SqlParameter("idgenero", idgenero);
            var consulta = this.context.Juegos.FromSqlRaw(sql, pamID);
            List<Juego> juegos = consulta.ToList();
            return juegos;
        }

        public Juego FindJuego(int IdJuego)
        {
            string sql = "SP_DETALLES_JUEGO @IDJuego";
            SqlParameter pamId = new SqlParameter("@IDJuego", IdJuego);
            var consulta = this.context.Juegos.FromSqlRaw(sql, pamId);
            Juego juego = consulta.AsEnumerable().FirstOrDefault();
            return juego;
        }

        public void RegistrarJuego(string nombre, int idGenero, string imagen, decimal precio, string descripcion, int idEditor)
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

        public void ModificarJuego(int idJuego, string nombre, int idGenero, string imagen, decimal precio, string descripcion, int idEditor)
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

        public async Task<Genero> FindGeneroAsync(int idGenero)
        {
            return await this.context.Generos.FirstOrDefaultAsync(z => z.IdGenero == idGenero);
        }

        public async Task<Editor> FindEditorAsync(int idEditor)
        {
            return await this.context.Editores.FirstOrDefaultAsync(z => z.IDEditor == idEditor);
        }

        public void CrearGenero(string nombre)
        {
            string sql = "SP_CREATE_GENERO @NombreGenero";
            SqlParameter pamNombre = new SqlParameter("NombreGenero", nombre);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre);
        }
        
        public void CrearEditor(string nombre)
        {
            string sql = "SP_CREATE_EDITOR @NombreEditor";
            SqlParameter pamNombre = new SqlParameter("NombreEditor", nombre);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre);
        }

        public void DeleteGenero(int idGenero)
        {
            string sql = "SP_DELETE_GENERO @IDGenero";
            SqlParameter pamid = new SqlParameter("@IDGenero", idGenero);
            this.context.Database.ExecuteSqlRaw(sql, pamid);
        }

        public void DeleteEditor(int idEditor)
        {
            string sql = "SP_DELETE_EDITOR @IDEditor";
            SqlParameter pamid = new SqlParameter("@IDEditor", idEditor);
            this.context.Database.ExecuteSqlRaw(sql, pamid);
        }

        public void ModificarGenero(int idGenero, string nombre)
        {
            string sql = "SP_MODIFICAR_GENERO @IDGenero, @NombreGenero";
            SqlParameter pamId = new SqlParameter("IDGenero", idGenero);
            SqlParameter pamNombre = new SqlParameter("NombreGenero", nombre);
            this.context.Database.ExecuteSqlRaw(sql, pamId, pamNombre);
        }

        public void ModificarEditor(int idEditor, string nombre)
        {
            string sql = "SP_MODIFICAR_EDITOR @IDEditor, @NombreEditor";
            SqlParameter pamId = new SqlParameter("IDEditor", idEditor);
            SqlParameter pamNombre = new SqlParameter("NombreEditor", nombre);
            this.context.Database.ExecuteSqlRaw(sql, pamId, pamNombre);
        }


        public async Task<List<Juego>> GetGrupoJuegosAsync(int posicion)
        {
            string sql = "SP_GRUPO_JUEGOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("posicion", posicion);
            var consulta = this.context.Juegos.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<int> GetNumeroJuegosAsync()
        {
            return await this.context.Juegos.CountAsync();
        }

        public async Task<List<Juego>> GetJuegosSessionAsync(List<int> juegos)
        {
            return await this.context.Juegos
                .Where(c => juegos.Contains(c.IdJuego))
                .ToListAsync();
        }

        public async Task<int> GetMaxIdPedidoAsync()
        {
            if (this.context.Pedidos.Count() == 0) return 1;
            return await this.context.Pedidos.MaxAsync(x => x.IDPedido) + 1;
        }

        public async Task<int> GetMaxIdDetallePedidoAsync()
        {
            if (this.context.DetallePedidos.Count() == 0) return 1;
            return await this.context.DetallePedidos.MaxAsync(x => x.IDDetallePedido) + 1;
        }

        public async Task<List<Juego>> GetProductosEnCarritoAsync(List<int> idsJuegos)
        {
            List<Juego> productosEnCarrito = await context.Juegos
                                                        .Where(p => idsJuegos.Contains(p.IdJuego))
                                                        .ToListAsync();
            return productosEnCarrito;
        }

        public async Task<Pedido> CreatePedidoAsync(int idusuario, List<Juego> carrito)
        {
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
            await this.context.Pedidos.AddAsync(pedido);
            await this.context.SaveChangesAsync();

            foreach (Juego p in carrito)
            {
                DetallesPedido detalle = new DetallesPedido
                {
                    IDDetallePedido = await GetMaxIdDetallePedidoAsync(),
                    IDPedido = pedido.IDPedido,
                    IDJuego = p.IdJuego,
                    Cantidad = 1,
                    PrecioUnitario = p.PrecioJuego
                };

                // Verificar si ya existe un DetallePedido con el mismo IdDetallePedido
                DetallesPedido existingDetalle = await this.context.DetallePedidos.FindAsync(detalle.IDDetallePedido);
                if (existingDetalle != null)
                {
                    // Actualizar el DetallePedido existente si es necesario
                    existingDetalle.IDPedido = detalle.IDPedido;
                    existingDetalle.IDJuego = detalle.IDJuego;
                    existingDetalle.Cantidad = detalle.Cantidad;
                    existingDetalle.PrecioUnitario = detalle.PrecioUnitario;
                }
                else
                {
                    // Agregar el nuevo DetallePedido al contexto si no existe
                    await this.context.AddAsync(detalle);
                    await this.context.SaveChangesAsync();

                }
            }

            await this.context.SaveChangesAsync();
            return pedido;
        }

        public async Task<List<DetallePedidoView>> GetProductosPedidoAsync(List<int> idpedidos)
        {
            return await this.context.DetallePedidoViews.Where(x => idpedidos.Contains(x.IdPedido)).ToListAsync();
        }

        public async Task<List<DetallePedidoView>> GetProductosPedidoUsuarioAsync(int idUsuario)
        {
            return await context.DetallePedidoViews
                .Where(d => d.IdUsuario == idUsuario)
                .ToListAsync();
        }

    }
}
