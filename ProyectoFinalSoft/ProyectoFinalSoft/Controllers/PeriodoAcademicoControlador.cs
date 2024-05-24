using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    [Authorize(Roles = "Coordinador")]
    public class PeriodoAcademicoControlador : Controller
    {
        private readonly AppDbContext _context;

        public PeriodoAcademicoControlador(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string periodoBusqueda)
        {

            var periodos = from periodo in _context.PeriodosAcademicos select periodo;

            if (!String.IsNullOrEmpty(periodoBusqueda))
            {
                periodos = periodos.Where(periodo => (periodo.periodoNombre + " " + periodo.periodoFechaInicio).Contains(periodoBusqueda));
            }

            periodos = periodos.OrderBy(periodo => periodo.periodoFechaInicio);

            return View(await periodos.ToListAsync());
        }

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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("periodoId,periodoFechaInicio,periodoFechaFin,periodoNombre,periodoEstado")] PeriodoAcademico periodoAcademico)
        {
            if (ModelState.IsValid)
            {
                if (periodoAcademico.periodoFechaInicio >= periodoAcademico.periodoFechaFin)
                {
                    ModelState.AddModelError("periodoFechaFin", "La fecha de fin debe ser mayor que la fecha de inicio.");
                    return View(periodoAcademico);
                }

                if (ExistePeriodoConMismasFechas(periodoAcademico.periodoId, periodoAcademico.periodoFechaInicio, periodoAcademico.periodoFechaFin))
                {
                    ModelState.AddModelError("", "Ya existe un periodo académico con las mismas fechas de inicio y fin.");
                    return View(periodoAcademico);
                }

                var diferenciaMeses = ((periodoAcademico.periodoFechaFin.Year - periodoAcademico.periodoFechaInicio.Year)
                                         * 12) + periodoAcademico.periodoFechaFin.Month - periodoAcademico.periodoFechaInicio.Month;
                if (diferenciaMeses !=6 && diferenciaMeses!= 3)
                {
                    ModelState.AddModelError("periodoFechaFin", "La duración del periodo académico debe ser de tres o seis meses.");
                    return View(periodoAcademico);
                }
                periodoAcademico.periodoEstado = 1;
                _context.Add(periodoAcademico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodoAcademico);
        }

        private bool ExistePeriodoConMismasFechas(int periodoId, DateOnly periodoFechaInicio, DateOnly periodoFechaFin)
        {
            return _context.PeriodosAcademicos.Any(p => p.periodoFechaInicio == periodoFechaInicio
            && p.periodoFechaFin == periodoFechaFin && p.periodoId != periodoId);
        }

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
                if (ExistePeriodoConMismasFechas(periodoAcademico.periodoId, periodoAcademico.periodoFechaInicio, periodoAcademico.periodoFechaFin))
                {
                    ModelState.AddModelError("", "Ya existe un periodo académico con las mismas fechas de inicio y fin.");
                    return View(periodoAcademico);
                }

                var diferenciaMeses = ((periodoAcademico.periodoFechaFin.Year - periodoAcademico.periodoFechaInicio.Year)
                         * 12) + periodoAcademico.periodoFechaFin.Month - periodoAcademico.periodoFechaInicio.Month;
                if (diferenciaMeses != 6 && diferenciaMeses != 3)
                {
                    ModelState.AddModelError("periodoFechaFin", "La duración del periodo académico debe ser de tres o seis meses.");
                    return View(periodoAcademico);
                }

                try
                {
                    periodoAcademico.periodoEstado = 1;
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodo = await _context.PeriodosAcademicos.FindAsync(id);
            if (periodo != null)
            {
                periodo.periodoEstado = periodo.periodoEstado == 0 ? 1 : 0;
                _context.PeriodosAcademicos.Update(periodo);
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
