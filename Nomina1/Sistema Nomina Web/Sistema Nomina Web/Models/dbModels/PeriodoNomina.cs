using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    [Table("PeriodoNomina")]
    public partial class PeriodoNomina
    {
        public PeriodoNomina()
        {
            Incidencia = new HashSet<Incidencium>();
            Nominas = new HashSet<Nomina>();
        }

        [Key]
        public int PeriodoNominaId { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaInicio { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaFin { get; set; }
        public int? TipoSalarioId { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Estado { get; set; }

        [ForeignKey("TipoSalarioId")]
        [InverseProperty("PeriodoNominas")]
        public virtual TipoSalario? TipoSalario { get; set; }
        [InverseProperty("PeriodoNomina")]
        public virtual ICollection<Incidencium> Incidencia { get; set; }
        [InverseProperty("PeriodoNomina")]
        public virtual ICollection<Nomina> Nominas { get; set; }
    }
}
