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
    public class TrabajadorController : Controller
    {
        private readonly DB_NominaContext _context;

        public TrabajadorController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: Trabajador
        public async Task<IActionResult> Index()
        {
            var dB_NominaContext = _context.Trabajadors.Include(t => t.Periodicidad).Include(t => t.TipoJornada).Include(t => t.TipoSalario);
            return View(await dB_NominaContext.ToListAsync());
        }

        // GET: Trabajador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trabajadors == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadors
                .Include(t => t.Periodicidad)
                .Include(t => t.TipoJornada)
                .Include(t => t.TipoSalario)
                .FirstOrDefaultAsync(m => m.TrabajadorId == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajador/Create
        public IActionResult Create()
        {
            ViewBag.TipoJornadaId = new SelectList(_context.TipoJornada, "TipoJornadaId", "Descripcion"); // Asegúrate de que "Nombre" sea el campo que contiene el nombre del tipo de jornada
            ViewBag.TipoSalarioId = new SelectList(_context.TipoSalarios, "TipoSalarioId", "Descripcion"); // "Nombre" debe ser el campo con el nombre del tipo de salario
            ViewBag.PeriodicidadId = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre"); // "Nombre" debe contener los valores como "Semanal", "Quincenal", etc.

            return View();
        }

        // POST: Trabajador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrabajadorId,Nombre,Rfc,Curp,Nss,FechaInicio,TipoJornadaId,TipoSalarioId,SalarioBase,PeriodicidadId,Activo")] Trabajador trabajador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trabajador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", trabajador.PeriodicidadId);
            ViewData["TipoJornadaId"] = new SelectList(_context.TipoJornada, "TipoJornadaId", "Descripcion", trabajador.TipoJornadaId);
            ViewData["TipoSalarioId"] = new SelectList(_context.TipoSalarios, "TipoSalarioId", "Descripcion", trabajador.TipoSalarioId);
            return View(trabajador);
        }

        // GET: Trabajador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trabajadors == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadors.FindAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", trabajador.PeriodicidadId);
            ViewData["TipoJornadaId"] = new SelectList(_context.TipoJornada, "TipoJornadaId", "Descripcion", trabajador.TipoJornadaId);
            ViewData["TipoSalarioId"] = new SelectList(_context.TipoSalarios, "TipoSalarioId", "Descripcion", trabajador.TipoSalarioId);
            return View(trabajador);
        }

        // POST: Trabajador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrabajadorId,Nombre,Rfc,Curp,Nss,FechaInicio,TipoJornadaId,TipoSalarioId,SalarioBase,PeriodicidadId,Activo")] Trabajador trabajador)
        {
            if (id != trabajador.TrabajadorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trabajador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrabajadorExists(trabajador.TrabajadorId))
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
            ViewData["PeriodicidadId"] = new SelectList(_context.Periodicidades, "PeriodicidadId", "Nombre", trabajador.PeriodicidadId);
            ViewData["TipoJornadaId"] = new SelectList(_context.TipoJornada, "TipoJornadaId", "Descripcion", trabajador.TipoJornadaId);
            ViewData["TipoSalarioId"] = new SelectList(_context.TipoSalarios, "TipoSalarioId", "Descripcion", trabajador.TipoSalarioId);
            return View(trabajador);
        }

        // GET: Trabajador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trabajadors == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadors
                .Include(t => t.Periodicidad)
                .Include(t => t.TipoJornada)
                .Include(t => t.TipoSalario)
                .FirstOrDefaultAsync(m => m.TrabajadorId == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // POST: Trabajador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trabajadors == null)
            {
                return Problem("Entity set 'DB_NominaContext.Trabajadors'  is null.");
            }
            var trabajador = await _context.Trabajadors.FindAsync(id);
            if (trabajador != null)
            {
                _context.Trabajadors.Remove(trabajador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrabajadorExists(int id)
        {
          return (_context.Trabajadors?.Any(e => e.TrabajadorId == id)).GetValueOrDefault();
        }
    }
}
