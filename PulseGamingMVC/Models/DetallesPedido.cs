using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("DetallesPedido")]
    public class DetallesPedido
    {
        [Key]
        [Column("IDDetallePedido")]
        public int IDDetallePedido { get; set; }
        [Column("IDJuego")]
        public int IDJuego { get; set; }
        public Juego Juego { get; set; }
        [Column("Total")]
        public double Total { get; set; }
        [Column("Cantidad")]
        public int Cantidad { get; set; }
        [Column("IDPedido")]
        public int IDPedido { get; set; }
    }
}
