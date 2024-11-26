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
    public class IncidenciasController : Controller
    {
        private readonly DB_NominaContext _context;

        public IncidenciasController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: Incidencias
        public async Task<IActionResult> Index()
        {
            var dB_NominaContext = _context.Incidencia.Include(i => i.PeriodoNomina).Include(i => i.Trabajador);
            return View(await dB_NominaContext.ToListAsync());
        }

        // GET: Incidencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Incidencia == null)
            {
                return NotFound();
            }

            var incidencium = await _context.Incidencia
                .Include(i => i.PeriodoNomina)
                .Include(i => i.Trabajador)
                .FirstOrDefaultAsync(m => m.IncidenciaId == id);
            if (incidencium == null)
            {
                return NotFound();
            }

            return View(incidencium);
        }

        // GET: Incidencias/Create
        public IActionResult Create()
        {
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId");
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "Nombre");
            return View();
        }

        // POST: Incidencias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncidenciaId,TrabajadorId,PeriodoNominaId,Faltas,HorasExtra,Fecha")] Incidencium incidencium)
        {
            if (ModelState.IsValid)
            {
                // Agrega valores predeterminados si es necesario
                incidencium.Faltas = incidencium.Faltas ?? 0;
                incidencium.HorasExtra = incidencium.HorasExtra ?? 0;

                _context.Add(incidencium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", incidencium.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", incidencium.TrabajadorId);
            return View(incidencium);
        }

        // GET: Incidencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Incidencia == null)
            {
                return NotFound();
            }

            var incidencium = await _context.Incidencia.FindAsync(id);
            if (incidencium == null)
            {
                return NotFound();
            }
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", incidencium.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", incidencium.TrabajadorId);
            return View(incidencium);
        }

        // POST: Incidencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidenciaId,TrabajadorId,PeriodoNominaId,Faltas,HorasExtra,Fecha")] Incidencium incidencium)
        {
            if (id != incidencium.IncidenciaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidencium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidenciumExists(incidencium.IncidenciaId))
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
            ViewData["PeriodoNominaId"] = new SelectList(_context.PeriodoNominas, "PeriodoNominaId", "PeriodoNominaId", incidencium.PeriodoNominaId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajadors, "TrabajadorId", "TrabajadorId", incidencium.TrabajadorId);
            return View(incidencium);
        }

        // GET: Incidencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Incidencia == null)
            {
                return NotFound();
            }

            var incidencium = await _context.Incidencia
                .Include(i => i.PeriodoNomina)
                .Include(i => i.Trabajador)
                .FirstOrDefaultAsync(m => m.IncidenciaId == id);
            if (incidencium == null)
            {
                return NotFound();
            }

            return View(incidencium);
        }

        // POST: Incidencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Incidencia == null)
            {
                return Problem("Entity set 'DB_NominaContext.Incidencia'  is null.");
            }
            var incidencium = await _context.Incidencia.FindAsync(id);
            if (incidencium != null)
            {
                _context.Incidencia.Remove(incidencium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidenciumExists(int id)
        {
          return (_context.Incidencia?.Any(e => e.IncidenciaId == id)).GetValueOrDefault();
        }
    }
}
