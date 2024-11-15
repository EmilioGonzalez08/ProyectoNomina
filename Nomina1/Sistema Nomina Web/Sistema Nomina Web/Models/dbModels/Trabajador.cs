using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    [Table("Trabajador")]
    public partial class Trabajador
    {
        public Trabajador()
        {
            Incidencia = new HashSet<Incidencium>();
            Nominas = new HashSet<Nomina>();
        }

        [Key]

        [Display(Name = "Numero de empleado")]
        public int TrabajadorId { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [Column("RFC")]
        [StringLength(13)]
        [Unicode(false)]
        public string Rfc { get; set; } = null!;
        [Column("CURP")]
        [StringLength(18)]
        [Unicode(false)]
        public string Curp { get; set; } = null!;
        [Column("NSS")]
        [StringLength(11)]
        [Unicode(false)]
        [Display(Name = "Numero de seguro social")]
        public string Nss { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Tipo Jornada")]
        public int? TipoJornadaId { get; set; }

        [Display(Name = "Tipo Salario")]
        public int? TipoSalarioId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]

        [Display(Name = "Salario Base")]
        public decimal SalarioBase { get; set; }

        [Display(Name = "Periodicidad de pago")]
        public int? PeriodicidadId { get; set; } // Clave foránea opcional

        [ForeignKey("PeriodicidadId")]
        public virtual Periodicidad? Periodicidad { get; set; } // Relación con Periodicidad

        public bool? Activo { get; set; }

        [ForeignKey("TipoJornadaId")]
        [InverseProperty("Trabajadors")]
        public virtual TipoJornadum? TipoJornada { get; set; }
        [ForeignKey("TipoSalarioId")]
        [InverseProperty("Trabajadors")]
        public virtual TipoSalario? TipoSalario { get; set; }
        [InverseProperty("Trabajador")]
        public virtual ICollection<Incidencium> Incidencia { get; set; }
        [InverseProperty("Trabajador")]
        public virtual ICollection<Nomina> Nominas { get; set; }
    }
}