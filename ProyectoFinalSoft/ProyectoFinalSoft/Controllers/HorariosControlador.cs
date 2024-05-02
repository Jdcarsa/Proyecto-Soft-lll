using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Fachada;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    public class HorariosControlador : Controller, IFachada
    {
        private readonly AppDbContext _context;

        public HorariosControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: HorariosControlador
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico).Include(h => h.programa);
            return View(await appDbContext.ToListAsync());
        }

        // GET: HorariosControlador/Details/5
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

        // GET: HorariosControlador/Create
        public IActionResult Create()
        {
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo");
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId");
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteApellido");
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre");
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId");
            return View();
        }

        // POST: HorariosControlador/Create
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
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteApellido", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // GET: HorariosControlador/Edit/5
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
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteApellido", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // POST: HorariosControlador/Edit/5
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
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo", horario.ambienteId);
            ViewData["CompetenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaId", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "docenteApellido", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre", horario.periodoAcademicoId);
            ViewData["ProgramaId"] = new SelectList(_context.Programas, "programaId", "programaId", horario.ProgramaId);
            return View(horario);
        }

        // GET: HorariosControlador/Delete/5
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

        // POST: HorariosControlador/Delete/5
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

        public Task<IActionResult> obtenerCompetencias()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> obtenerProgramas()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> obtenerDocentes()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> obtenerPeridosAcademicos()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> obtenerAmbientes()
        {
            throw new NotImplementedException();
        }
    }
}
