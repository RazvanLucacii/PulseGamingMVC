using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("Juego")]
    public class Juego
    {
        [Key]
        [Column("IDJuego")]
        public int IdJuego { get; set; }
        [Column("NombreJuego")]
        public string? NombreJuego { get; set; }
        [Column("IDGenero")]
        public int IDGenero { get; set; }
        [Column("ImagenJuego")]
        public string? ImagenJuego { get; set; }
        [Column("PrecioJuego")]
        public double PrecioJuego { get; set; }
        [Column("DescripcionJuego")]
        public string? Descripcion { get; set; }
        [Column("IDEditor")]
        public int IdEditor { get; set; }

    }
}
