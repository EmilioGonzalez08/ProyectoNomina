using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Nomina_Web.Models.dbModels
{
    public partial class TipoJornadum
    {
        public TipoJornadum()
        {
            Trabajadors = new HashSet<Trabajador>();
        }

        [Key]
        public int TipoJornadaId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Descripcion { get; set; } = null!;
        [Column(TypeName = "decimal(5, 2)")]
        public decimal HorasJornada { get; set; }

        [InverseProperty("TipoJornada")]
        public virtual ICollection<Trabajador> Trabajadors { get; set; }
    }
}
