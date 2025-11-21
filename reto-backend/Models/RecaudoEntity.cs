using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetoBackend.Models
{
    [Table("Recaudos", Schema = "Jeremy_Reto")]
    public class RecaudoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 📅 Fecha del registro
        [Required]
        public DateTime Fecha { get; set; }

        // 🏁 Nombre de la estación
        [Required]
        [MaxLength(100)]
        public string EstacionNombre { get; set; } = string.Empty;

        // 🔄 Sentido del tráfico
        [MaxLength(50)]
        public string? Sentido { get; set; }

        // 🚗 Categoría del vehículo
        [MaxLength(50)]
        public string? Categoria { get; set; }

        // 🕒 Hora del conteo o recaudo (0–23)
        [Range(0, 23)]
        public int? Hora { get; set; }

        // 🔢 Cantidad de vehículos (requerido)
        [Required]
        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; }

        // 💰 Valor total recaudado
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Valor { get; set; }
    }
}
