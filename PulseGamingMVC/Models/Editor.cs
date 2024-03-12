using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseGamingMVC.Models
{
    [Table("Editor")]
    public class Editor
    {
        [Key]
        [Column("IDEditor")]
        public int IDEditor { get; set; }
        [Column("NombreEditor")]
        public string NombreEditor { get; set; }
    }
}
