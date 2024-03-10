using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Column("IDPedido")]
        public int IDPedido { get; set; }
        [Column("FechaPedido")]
        public string FechaPedido { get; set; }
        [Column("Ciudad")]
        public string Ciudad { get; set; }
        [Column("Pais")]
        public string Pais { get; set; }
        [Column("IDUsuario")]
        public int IDUsuario { get; set; }
        [Column("Total")]
        public double Total { get; set; }
        [Column("TotalProducto")]
        public string TotalProducto { get; set; }

        public List<DetallesPedido> IDDetallePedido { get; set; }
    }
}
