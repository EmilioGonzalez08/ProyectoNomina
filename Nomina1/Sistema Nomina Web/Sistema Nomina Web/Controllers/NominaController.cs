using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Nomina_Web.Models.dbModels;

namespace Sistema_Nomina_Web.Controllers
{
    public class NominaController : Controller
    {
        private readonly DB_NominaContext _context;

        public NominaController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: Nomina
        public async Task<IActionResult> Index()
        {
            var dB_NominaContext = _context.Nominas
                                           .Include(n => n.PeriodoNomina)
                                           .Include(n => n.Trabajador)
                                           .Include(n => n.Incidencia); // Asegúrate de incluir incidencias
            return View(await dB_NominaContext.ToListAsync());
        }


        // GET: Nomina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas
                .Include(n => n.PeriodoNomina)
                .Include(n => n.Trabajador)
                .FirstOrDefaultAsync(m => m.NominaId == id);
            if (nomina == null)
            {
                return NotFound();
            }

            return View(nomina);
        }

        // GET: Nomina/Create
        public IActionResult Create()
        {
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "Nombre");
            ViewData["SalarioBase"] = new SelectList(_context.Trabajadors, "SalarioBase", "SalarioBase");
            return View();
        }

        // POST: Nomina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NominaId,TrabajadorId,PeriodoNominaId,ImporteHorasExtra,DescuentoFaltas,Isr,Imss,OtrasDeducciones,TotalPercepciones,TotalDeducciones,SalarioNeto,FechaCalculo")] Nomina nomina)
        {
            if (ModelState.IsValid)
            {
                // Obtener el trabajador incluyendo su periodicidad
                var trabajador = await _context.Trabajadors
                    .Include(t => t.Periodicidad)
                    .FirstOrDefaultAsync(t => t.TrabajadorId == nomina.TrabajadorId);

                // Obtener la incidencia correspondiente
                var incidenciaEncontrada = await _context.Incidencia
                    .FirstOrDefaultAsync(i => i.TrabajadorId == nomina.TrabajadorId &&
                                            i.PeriodoNominaId == nomina.PeriodoNominaId);

                if (trabajador != null)
                {
                    // Obtener el factor de periodicidad - asumiendo que la propiedad es PeriodicidadId o Id
                    decimal factorPeriodicidad = ObtenerFactorPeriodicidad(trabajador.Periodicidad.PeriodicidadId.ToString());

                    // Calcular salario según periodicidad
                    decimal salarioSegunPeriodicidad = trabajador.SalarioBase * factorPeriodicidad;

                    // Calcular salario diario (basado en salario mensual)
                    decimal salarioDiario = trabajador.SalarioBase / 30m;
                    // Calcular salario por hora (día laboral de 8 horas)
                    decimal salarioPorHora = salarioDiario / 8m;

                    if (incidenciaEncontrada != null)
                    {
                        nomina.IncidenciaId = incidenciaEncontrada.IncidenciaId;

                        // Calcular importe de horas extra (doble del valor normal)
                        nomina.ImporteHorasExtra = incidenciaEncontrada.HorasExtra * (salarioPorHora * 2);

                        // Calcular descuento por faltas (día completo)
                        nomina.DescuentoFaltas = incidenciaEncontrada.Faltas * salarioDiario;
                    }

                    // Calcular percepciones totales según periodicidad
                    nomina.TotalPercepciones = salarioSegunPeriodicidad + (nomina.ImporteHorasExtra ?? 0m);

                    // Calcular ISR según periodicidad
                    decimal salarioAnual = trabajador.SalarioBase * 12m;
                    decimal isrMensual;

                    if (salarioAnual <= 123580m)
                        isrMensual = (nomina.TotalPercepciones ?? 0m) * 0.0192m; // 1.92%
                    else if (salarioAnual <= 249243m)
                        isrMensual = (nomina.TotalPercepciones ?? 0m) * 0.064m; // 6.40%
                    else if (salarioAnual <= 392841m)
                        isrMensual = (nomina.TotalPercepciones ?? 0m) * 0.1088m; // 10.88%
                    else if (salarioAnual <= 750000m)
                        isrMensual = (nomina.TotalPercepciones ?? 0m) * 0.16m; // 16%
                    else
                        isrMensual = (nomina.TotalPercepciones ?? 0m) * 0.1792m; // 17.92%

                    // Ajustar ISR según periodicidad
                    nomina.Isr = isrMensual * factorPeriodicidad;

                    // Calcular IMSS según periodicidad
                    decimal imssMensual = (nomina.TotalPercepciones ?? 0m) * 0.02775m;
                    nomina.Imss = imssMensual * factorPeriodicidad;

                    // Calcular deducciones totales
                    nomina.TotalDeducciones = (nomina.Isr ?? 0m) +
                                            (nomina.Imss ?? 0m) +
                                            (nomina.DescuentoFaltas ?? 0m) +
                                            (nomina.OtrasDeducciones ?? 0m);

                    // Calcular salario neto
                    nomina.SalarioNeto = (nomina.TotalPercepciones ?? 0m) - (nomina.TotalDeducciones ?? 0m);
                    nomina.FechaCalculo = DateTime.Now;
                }

                _context.Add(nomina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        private decimal ObtenerFactorPeriodicidad(string periodicidadId)
        {
            return periodicidadId switch
            {
                "1" => 1m / 30m,        // Diario (1/30 del salario mensual)
                "2" => 7m / 30m,        // Semanal (7/30 del salario mensual)
                "3" => 14m / 30m,       // Catorcenal (14/30 del salario mensual)
                "4" => 15m / 30m,       // Quincenal (15/30 del salario mensual)
                "5" => 1m,            // Mensual (salario completo)
                _ => 1m                // Por defecto mensual
            };
        }

        // GET: Nomina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas.FindAsync(id);
            if (nomina == null)
            {
                return NotFound();
            }
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        // POST: Nomina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NominaId,TrabajadorId,PeriodoNominaId,SalarioBase,HorasExtra,ImporteHorasExtra,DescuentoFaltas,Isr,Imss,OtrasDeducciones,TotalPercepciones,TotalDeducciones,SalarioNeto,FechaCalculo")] Nomina nomina)
        {
            if (id != nomina.NominaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nomina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NominaExists(nomina.NominaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", nomina.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", nomina.TrabajadorId);
            return View(nomina);
        }

        // GET: Nomina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nominas == null)
            {
                return NotFound();
            }

            var nomina = await _context.Nominas
                .Include(n => n.PeriodoNomina)
                .Include(n => n.Trabajador)
                .FirstOrDefaultAsync(m => m.NominaId == id);
            if (nomina == null)
            {
                return NotFound();
            }

            return View(nomina);
        }

        // POST: Nomina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nominas == null)
            {
                return Problem("Entity set 'DB_NominaContext.Nominas'  is null.");
            }
            var nomina = await _context.Nominas.FindAsync(id);
            if (nomina != null)
            {
                _context.Nominas.Remove(nomina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NominaExists(int id)
        {
          return (_context.Nominas?.Any(e => e.NominaId == id)).GetValueOrDefault();
        }
    }
}
