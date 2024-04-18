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
        public DateTime FechaPedido { get; set; }

        [Column("IDUsuario")]
        public int IDUsuario { get; set; }

        [Column("Total")]
        public decimal Total { get; set; }
    }
}
