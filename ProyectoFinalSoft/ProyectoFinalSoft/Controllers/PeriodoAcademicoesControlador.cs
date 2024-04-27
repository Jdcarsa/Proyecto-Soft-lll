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
    public class PeriodoAcademicoesControlador : Controller
    {
        private readonly AppDbContext _context;

        public PeriodoAcademicoesControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: PeriodoAcademicoesControlador
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodosAcademicos.ToListAsync());
        }

        // GET: PeriodoAcademicoesControlador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodoAcademico = await _context.PeriodosAcademicos
                .FirstOrDefaultAsync(m => m.periodoId == id);
            if (periodoAcademico == null)
            {
                return NotFound();
            }

            return View(periodoAcademico);
        }

        // GET: PeriodoAcademicoesControlador/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PeriodoAcademicoesControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("periodoId,periodoFechaInicio,periodoFechaFin,periodoNombre,periodoEstado")] PeriodoAcademico periodoAcademico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodoAcademico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodoAcademico);
        }

        // GET: PeriodoAcademicoesControlador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodoAcademico = await _context.PeriodosAcademicos.FindAsync(id);
            if (periodoAcademico == null)
            {
                return NotFound();
            }
            return View(periodoAcademico);
        }

        // POST: PeriodoAcademicoesControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("periodoId,periodoFechaInicio,periodoFechaFin,periodoNombre,periodoEstado")] PeriodoAcademico periodoAcademico)
        {
            if (id != periodoAcademico.periodoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodoAcademico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodoAcademicoExists(periodoAcademico.periodoId))
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
            return View(periodoAcademico);
        }

        // GET: PeriodoAcademicoesControlador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodoAcademico = await _context.PeriodosAcademicos
                .FirstOrDefaultAsync(m => m.periodoId == id);
            if (periodoAcademico == null)
            {
                return NotFound();
            }

            return View(periodoAcademico);
        }

        // POST: PeriodoAcademicoesControlador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodoAcademico = await _context.PeriodosAcademicos.FindAsync(id);
            if (periodoAcademico != null)
            {
                _context.PeriodosAcademicos.Remove(periodoAcademico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodoAcademicoExists(int id)
        {
            return _context.PeriodosAcademicos.Any(e => e.periodoId == id);
        }
    }
}
