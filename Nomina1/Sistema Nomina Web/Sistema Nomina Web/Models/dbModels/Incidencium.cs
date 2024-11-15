using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    public partial class Incidencium
    {
        [Key]
        public int IncidenciaId { get; set; }
        public int? TrabajadorId { get; set; }
        public int? PeriodoNominaId { get; set; }
        public int? Faltas { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? HorasExtra { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [ForeignKey("PeriodoNominaId")]
        [InverseProperty("Incidencia")]
        public virtual PeriodoNomina? PeriodoNomina { get; set; }
        [ForeignKey("TrabajadorId")]
        [InverseProperty("Incidencia")]
        public virtual Trabajador? Trabajador { get; set; }

        // Nueva propiedad de navegación
        public virtual ICollection<Nomina> Nominas { get; set; } = new List<Nomina>();
    }
}
