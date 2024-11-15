using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    [Table("Nomina")]
    public partial class Nomina
    {
        [Key]
        public int NominaId { get; set; }
        public int? TrabajadorId { get; set; }
        public int? PeriodoNominaId { get; set; }
        public int? IncidenciaId { get; set; } // Nueva propiedad
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ImporteHorasExtra { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? DescuentoFaltas { get; set; }
        [Column("ISR", TypeName = "decimal(10, 2)")]
        public decimal? Isr { get; set; }
        [Column("IMSS", TypeName = "decimal(10, 2)")]
        public decimal? Imss { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? OtrasDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalPercepciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SalarioNeto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaCalculo { get; set; }

        [ForeignKey("PeriodoNominaId")]
        [InverseProperty("Nominas")]
        public virtual PeriodoNomina? PeriodoNomina { get; set; }

        [ForeignKey("TrabajadorId")]
        [InverseProperty("Nominas")]
        public virtual Trabajador? Trabajador { get; set; }

        [ForeignKey("IncidenciaId")]
        public virtual Incidencium? Incidencia { get; set; }

        // Propiedades calculadas
        [NotMapped]
        public decimal SalarioBase => Trabajador?.SalarioBase ?? 0;

        [NotMapped]
        public decimal? HorasExtra => Incidencia?.HorasExtra ?? 0;

        [NotMapped]
        public int? Faltas => Incidencia?.Faltas ?? 0;

        [NotMapped] // Esto indica que no se mapea a la base de datos
        public decimal SalarioSegunPeriodicidad
        {
            get
            {
                if (Trabajador?.Periodicidad == null) return 0;

                decimal factor = Trabajador.Periodicidad.PeriodicidadId switch
                {
                    1 => 1m / 30m,        // Diario
                    2 => 7m / 30m,        // Semanal
                    3 => 14m / 30m,       // Catorcenal
                    4 => 15m / 30m,       // Quincenal
                    5 => 1m,              // Mensual
                    _ => 1m               // Por defecto mensual
                };

                return Trabajador.SalarioBase * factor;
            }
        }

    }

}
