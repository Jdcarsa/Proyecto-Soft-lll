using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    public class ProgramasControlador : Controller
    {
        private readonly AppDbContext _context;

        public ProgramasControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProgramasControlador
        public async Task<IActionResult> Index()
        {
            return View(await _context.Programas.ToListAsync());
        }

        // GET: ProgramasControlador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programa = await _context.Programas
                .FirstOrDefaultAsync(m => m.programaId == id);
            if (programa == null)
            {
                return NotFound();
            }

            return View(programa);
        }

        // GET: ProgramasControlador/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgramasControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("programaId,programaNombre,programaEstado")] Programa programa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programa);
        }

        // GET: ProgramasControlador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programa = await _context.Programas.FindAsync(id);
            if (programa == null)
            {
                return NotFound();
            }
            return View(programa);
        }

        // POST: ProgramasControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("programaId,programaNombre,programaEstado")] Programa programa)
        {
            if (id != programa.programaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramaExists(programa.programaId))
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
            return View(programa);
        }

        // GET: ProgramasControlador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programa = await _context.Programas
                .FirstOrDefaultAsync(m => m.programaId == id);
            if (programa == null)
            {
                return NotFound();
            }

            return View(programa);
        }

        // POST: ProgramasControlador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programa = await _context.Programas.FindAsync(id);
            if (programa != null)
            {
                _context.Programas.Remove(programa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramaExists(int id)
        {
            return _context.Programas.Any(e => e.programaId == id);
        }
    }
}
