using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Nomina_Web.Models.dbModels;

namespace Sistema_Nomina_Web.Controllers
{
    [Authorize]
    public class PeriodoNominaController : Controller
    {
        private readonly DB_NominaContext _context;

        public PeriodoNominaController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: PeriodoNomina
        public async Task<IActionResult> Index()
        {
            // Cambiado para incluir Periodicidad en lugar de TipoSalario
            var dB_NominaContext = _context.PeriodoNominas.Include(p => p.Periodicidad);
            return View(await dB_NominaContext.ToListAsync());
        }

        // GET: PeriodoNomina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PeriodoNominas == null)
            {
                return NotFound();
            }

            var periodoNomina = await _context.PeriodoNominas
                .Include(p => p.Periodicidad)
                .FirstOrDefaultAsync(m => m.PeriodoNominaId == id);
            if (periodoNomina == null)
            {
                return NotFound();
            }

            return View(periodoNomina);
        }

        // GET: PeriodoNomina/Create
        public IActionResult Create()
        {
            // Actualizado para cargar Periodicidad
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre");
            return View();
        }

        // POST: PeriodoNomina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PeriodoNominaId,FechaInicio,FechaFin,PeriodicidadId,Estado")] PeriodoNomina periodoNomina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodoNomina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", periodoNomina.PeriodicidadId);
            return View(periodoNomina);
        }

        // GET: PeriodoNomina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PeriodoNominas == null)
            {
                return NotFound();
            }

            var periodoNomina = await _context.PeriodoNominas.FindAsync(id);
            if (periodoNomina == null)
            {
                return NotFound();
            }
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", periodoNomina.PeriodicidadId);
            return View(periodoNomina);
        }

        // POST: PeriodoNomina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PeriodoNominaId,FechaInicio,FechaFin,PeriodicidadId,Estado")] PeriodoNomina periodoNomina)
        {
            if (id != periodoNomina.PeriodoNominaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodoNomina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodoNominaExists(periodoNomina.PeriodoNominaId))
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
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", periodoNomina.PeriodicidadId);
            return View(periodoNomina);
        }

        // GET: PeriodoNomina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PeriodoNominas == null)
            {
                return NotFound();
            }

            var periodoNomina = await _context.PeriodoNominas
                .Include(p => p.Periodicidad)
                .FirstOrDefaultAsync(m => m.PeriodoNominaId == id);
            if (periodoNomina == null)
            {
                return NotFound();
            }

            return View(periodoNomina);
        }

        // POST: PeriodoNomina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PeriodoNominas == null)
            {
                return Problem("Entity set 'DB_NominaContext.PeriodoNominas' is null.");
            }
            var periodoNomina = await _context.PeriodoNominas.FindAsync(id);
            if (periodoNomina != null)
            {
                _context.PeriodoNominas.Remove(periodoNomina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodoNominaExists(int id)
        {
            return (_context.PeriodoNominas?.Any(e => e.PeriodoNominaId == id)).GetValueOrDefault();
        }
    }
}
