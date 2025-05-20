using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoliciaApi.Models
{
    [Table("CLIENTE", Schema = "SYSTEM")]

    public class Cliente
    {
        [Key]
        [Column("CLIENTE_ID")]
        public int ClienteId { get; set; }

        [Required]
        [Column("NOMBRE")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Column("APELLIDO")]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Column("CORREO")]
        [StringLength(150)]
        public string Correo { get; set; }

        [Column("TELEFONO")]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Column("DIRECCION")]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime? FechaRegistro { get; set; }
    }
}
