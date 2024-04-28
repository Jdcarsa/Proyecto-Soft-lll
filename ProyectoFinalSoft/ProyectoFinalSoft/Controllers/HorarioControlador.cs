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
    public class HorarioControlador : Controller
    {
        private readonly AppDbContext _context;

        public HorarioControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: HorarioControlador
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico).Include(h => h.programa);
            return View(await appDbContext.ToListAsync());
        }

        // GET: HorarioControlador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.ambiente)
                .Include(h => h.competencia)
                .Include(h => h.docente)
                .Include(h => h.periodoAcademico)
                .Include(h => h.programa)
                .FirstOrDefaultAsync(m => m.horarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: HorarioControlador/Create
        public IActionResult Create()
        {
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteId");
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId");
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteId");
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoId");
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId");
            return View();
        }

        // POST: HorarioControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("horarioId,horarioDia,horarioHoraInicio,horarioHoraFin,horarioDuracion,horarioEstado,ambienteId,docenteId,periodoAcademicoId,ProgramaId,CompetenciaId")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteId", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteId", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoId", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // GET: HorarioControlador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteId", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteId", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoId", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // POST: HorarioControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("horarioId,horarioDia,horarioHoraInicio,horarioHoraFin,horarioDuracion,horarioEstado,ambienteId,docenteId,periodoAcademicoId,ProgramaId,CompetenciaId")] Horario horario)
        {
            if (id != horario.horarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.horarioId))
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
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteId", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteId", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoId", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // GET: HorarioControlador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.ambiente)
                .Include(h => h.competencia)
                .Include(h => h.docente)
                .Include(h => h.periodoAcademico)
                .Include(h => h.programa)
                .FirstOrDefaultAsync(m => m.horarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: HorarioControlador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.horarioId == id);
        }
    }
}
