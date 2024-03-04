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
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Salt")]
        public string Salt { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Telefono")]
        public int Telefono { get; set; }
        //UNA VENTAJA CON EF ES QUE LOS TIPOS DE 
        //DATOS VARBINARY, BLOB, CLOB SON CONVERTIDOS
        //AUTOMATICAMENTE A byte[]
        [Column("Password")]
        public byte[] Password { get; set; }
    }
}