using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    [Table("TipoSalario")]
    public partial class TipoSalario
    {
        public TipoSalario()
        {
            PeriodoNominas = new HashSet<PeriodoNomina>();
            Trabajadors = new HashSet<Trabajador>();
        }

        [Key]
        public int TipoSalarioId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Descripcion { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Periodicidad { get; set; } = null!;

        [InverseProperty("TipoSalario")]
        public virtual ICollection<PeriodoNomina> PeriodoNominas { get; set; }
        [InverseProperty("TipoSalario")]
        public virtual ICollection<Trabajador> Trabajadors { get; set; }
    }
}
