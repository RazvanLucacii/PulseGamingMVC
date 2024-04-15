using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("DetallesPedidoView")]
    public class DetallePedidoView
    {
        [Key]
        [Column("IDDetallePedido")]
        public int IdDetallePedido { get; set; }

        [Column("IDPedido")]
        public int IdPedido { get; set; }

        [Column("IDJuego")]
        public int IdJuego { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("PrecioUnitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("NOMBRE_JUEGO")]
        public string NombreJuego { get; set; }

        [Column("TOTAL_DETALLE")]
        public decimal TotalDetalle { get; set; }

        [Column("TOTAL_PEDIDO")]
        public decimal TotalPedido { get; set; }
        [Column("IDUsuario")]
        public int IdUsuario { get; set; }

    }
}
