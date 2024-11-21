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
    public class TipoSalarioController : Controller
    {
        private readonly DB_NominaContext _context;

        public TipoSalarioController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: TipoSalario
        public async Task<IActionResult> Index()
        {
              return _context.TipoSalarios != null ? 
                          View(await _context.TipoSalarios.ToListAsync()) :
                          Problem("Entity set 'DB_NominaContext.TipoSalarios'  is null.");
        }

        // GET: TipoSalario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoSalarios == null)
            {
                return NotFound();
            }

            var tipoSalario = await _context.TipoSalarios
                .FirstOrDefaultAsync(m => m.TipoSalarioId == id);
            if (tipoSalario == null)
            {
                return NotFound();
            }

            return View(tipoSalario);
        }

        // GET: TipoSalario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSalario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoSalarioId,Descripcion,Periodicidad")] TipoSalario tipoSalario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoSalario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSalario);
        }

        // GET: TipoSalario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoSalarios == null)
            {
                return NotFound();
            }

            var tipoSalario = await _context.TipoSalarios.FindAsync(id);
            if (tipoSalario == null)
            {
                return NotFound();
            }
            return View(tipoSalario);
        }

        // POST: TipoSalario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoSalarioId,Descripcion,Periodicidad")] TipoSalario tipoSalario)
        {
            if (id != tipoSalario.TipoSalarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoSalario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSalarioExists(tipoSalario.TipoSalarioId))
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
            return View(tipoSalario);
        }

        // GET: TipoSalario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoSalarios == null)
            {
                return NotFound();
            }

            var tipoSalario = await _context.TipoSalarios
                .FirstOrDefaultAsync(m => m.TipoSalarioId == id);
            if (tipoSalario == null)
            {
                return NotFound();
            }

            return View(tipoSalario);
        }

        // POST: TipoSalario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoSalarios == null)
            {
                return Problem("Entity set 'DB_NominaContext.TipoSalarios'  is null.");
            }
            var tipoSalario = await _context.TipoSalarios.FindAsync(id);
            if (tipoSalario != null)
            {
                _context.TipoSalarios.Remove(tipoSalario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSalarioExists(int id)
        {
          return (_context.TipoSalarios?.Any(e => e.TipoSalarioId == id)).GetValueOrDefault();
        }
    }
}
