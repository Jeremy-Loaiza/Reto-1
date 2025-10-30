using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetoBackend.Models
{
    [Table("recaudos", Schema = "Jeremy_Reto")]
    public class RecaudoEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        [MaxLength(50)]
        public string EstacionNombre { get; set; }

        public string? Sentido { get; set; }

        public string? Categoria { get; set; }

        public int? Hora { get; set; }

        public int Cantidad { get; set; }

        public decimal Valor { get; set; }
    }
}
