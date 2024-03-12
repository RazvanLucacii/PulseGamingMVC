using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("Genero")]
    public class Genero
    {
        [Key]
        [Column("IDGenero")]
        public int IdGenero { get; set; }
        [Column("NombreGenero")]
        public string NombreGenero { get; set; }
    }
}
