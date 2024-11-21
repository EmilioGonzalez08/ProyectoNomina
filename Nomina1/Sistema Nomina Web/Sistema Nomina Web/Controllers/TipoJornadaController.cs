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
    public class TipoJornadaController : Controller
    {
        private readonly DB_NominaContext _context;

        public TipoJornadaController(DB_NominaContext context)
        {
            _context = context;
        }

        // GET: TipoJornada
        public async Task<IActionResult> Index()
        {
              return _context.TipoJornada != null ? 
                          View(await _context.TipoJornada.ToListAsync()) :
                          Problem("Entity set 'DB_NominaContext.TipoJornada'  is null.");
        }

        // GET: TipoJornada/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoJornada == null)
            {
                return NotFound();
            }

            var tipoJornadum = await _context.TipoJornada
                .FirstOrDefaultAsync(m => m.TipoJornadaId == id);
            if (tipoJornadum == null)
            {
                return NotFound();
            }

            return View(tipoJornadum);
        }

        // GET: TipoJornada/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoJornada/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoJornadaId,Descripcion,HorasJornada")] TipoJornadum tipoJornadum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoJornadum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoJornadum);
        }

        // GET: TipoJornada/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoJornada == null)
            {
                return NotFound();
            }

            var tipoJornadum = await _context.TipoJornada.FindAsync(id);
            if (tipoJornadum == null)
            {
                return NotFound();
            }
            return View(tipoJornadum);
        }

        // POST: TipoJornada/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoJornadaId,Descripcion,HorasJornada")] TipoJornadum tipoJornadum)
        {
            if (id != tipoJornadum.TipoJornadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoJornadum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoJornadumExists(tipoJornadum.TipoJornadaId))
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
            return View(tipoJornadum);
        }

        // GET: TipoJornada/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoJornada == null)
            {
                return NotFound();
            }

            var tipoJornadum = await _context.TipoJornada
                .FirstOrDefaultAsync(m => m.TipoJornadaId == id);
            if (tipoJornadum == null)
            {
                return NotFound();
            }

            return View(tipoJornadum);
        }

        // POST: TipoJornada/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoJornada == null)
            {
                return Problem("Entity set 'DB_NominaContext.TipoJornada'  is null.");
            }
            var tipoJornadum = await _context.TipoJornada.FindAsync(id);
            if (tipoJornadum != null)
            {
                _context.TipoJornada.Remove(tipoJornadum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoJornadumExists(int id)
        {
          return (_context.TipoJornada?.Any(e => e.TipoJornadaId == id)).GetValueOrDefault();
        }
    }
}
