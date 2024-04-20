using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulseGamingMVC.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("IDUsuario")]
        public int IdUsuario { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Telefono")]
        public int Telefono { get; set; }
        [Column("IDRole")]
        public int IDRole { get; set; }
    }
}