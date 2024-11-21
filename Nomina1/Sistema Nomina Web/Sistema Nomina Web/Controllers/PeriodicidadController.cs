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
    public class PeriodicidadController : Controller
    {
        private readonly DB_NominaContext _context;

        public PeriodicidadController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: Periodicidad
        public async Task<IActionResult> Index()
        {
              return _context.Periodicidades != null ? 
                          View(await _context.Periodicidades.ToListAsync()) :
                          Problem("Entity set 'DB_NominaContext.Periodicidades'  is null.");
        }

        // GET: Periodicidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Periodicidades == null)
            {
                return NotFound();
            }

            var periodicidad = await _context.Periodicidades
                .FirstOrDefaultAsync(m => m.PeriodicidadId == id);
            if (periodicidad == null)
            {
                return NotFound();
            }

            return View(periodicidad);
        }

        // GET: Periodicidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Periodicidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PeriodicidadId,Nombre")] Periodicidad periodicidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodicidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodicidad);
        }

        // GET: Periodicidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Periodicidades == null)
            {
                return NotFound();
            }

            var periodicidad = await _context.Periodicidades.FindAsync(id);
            if (periodicidad == null)
            {
                return NotFound();
            }
            return View(periodicidad);
        }

        // POST: Periodicidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PeriodicidadId,Nombre")] Periodicidad periodicidad)
        {
            if (id != periodicidad.PeriodicidadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodicidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodicidadExists(periodicidad.PeriodicidadId))
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
            return View(periodicidad);
        }

        // GET: Periodicidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Periodicidades == null)
            {
                return NotFound();
            }

            var periodicidad = await _context.Periodicidades
                .FirstOrDefaultAsync(m => m.PeriodicidadId == id);
            if (periodicidad == null)
            {
                return NotFound();
            }

            return View(periodicidad);
        }

        // POST: Periodicidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Periodicidades == null)
            {
                return Problem("Entity set 'DB_NominaContext.Periodicidades'  is null.");
            }
            var periodicidad = await _context.Periodicidades.FindAsync(id);
            if (periodicidad != null)
            {
                _context.Periodicidades.Remove(periodicidad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodicidadExists(int id)
        {
          return (_context.Periodicidades?.Any(e => e.PeriodicidadId == id)).GetValueOrDefault();
        }
    }
}
