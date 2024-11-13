using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sistema_Nomina_Web.Models.dbModels;

[Table("Periodicidad")]
public class Periodicidad
{
    [Key]
    public int PeriodicidadId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<PeriodoNomina> PeriodoNominas { get; set; } = new HashSet<PeriodoNomina>();
}