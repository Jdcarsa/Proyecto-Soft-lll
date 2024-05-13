using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoFinalSoft.Fachada;
using ProyectoFinalSoft.Json;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    public class HorariosControlador : Controller, IFachada
    {
        private readonly AppDbContext _context;
        private readonly DocenteServicio _docenteServicio;

        public HorariosControlador(AppDbContext context, DocenteServicio docenteServicio)
        {
            _context = context;
            _docenteServicio = docenteServicio;
        }

        // GET: HorariosControlador
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
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
            obtenerAmbientes();
            obtenerDocentes();
            obtenerPeridosAcademicos();
            obtenerCompetencias();

            return View();
        }

        // POST: HorariosControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("horarioId,horarioDia,horarioHoraInicio,horarioHoraFin,horarioDuracion,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
        {

            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            obtenerTodos(horario);
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
            obtenerTodos(horario);
            return View(horario);
        }

        // POST: HorariosControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("horarioId,horarioDia,horarioHoraInicio,horarioHoraFin,horarioDuracion,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
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
            obtenerTodos(horario);
           
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

        public void obtenerCompetencias()
        {
            ViewData["competenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaNombre");
        }


        public void obtenerDocentes()
        {
            ViewData["docenteId"] = _docenteServicio.ObtenerDocentes();
        }

        public void obtenerPeridosAcademicos()
        {
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre");
        }

        public void obtenerAmbientes()
        {
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo");
        }

        public void obtenerTodos(Horario horario)
        {
            ViewData["ambienteId"] = new SelectList(_context.Ambientes, "ambienteId", "ambienteCodigo", horario.ambienteId);
            ViewData["competenciaId"] = new SelectList(_context.Competencias, "competenciaId", "competenciaNombre", horario.CompetenciaId);
            ViewData["docenteId"] = new SelectList(_context.Docentes, "docenteId", "infoCompleta", horario.docenteId);
            ViewData["periodoAcademicoId"] = new SelectList(_context.PeriodosAcademicos, "periodoId", "periodoNombre", horario.periodoAcademicoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task guardarDatosProgComp()
        {
            var rutaAlArchivo = "C:\\Users\\ideapad330S\\Documents\\GitHub\\Proyecto-Soft-lll\\ProyectoFinalSoft\\ProyectoFinalSoft\\Json\\Programas.json";

            // Usa 'await' para esperar a que la lectura del archivo se complete
            var contenidoDeArchivo = await System.IO.File.ReadAllTextAsync(rutaAlArchivo);

            // Deserializa el JSON a una lista de programas
            var rootObject = JsonConvert.DeserializeObject<RootObject>(contenidoDeArchivo);
            var programas = rootObject.programas;

            foreach (var programa in programas)
            {
                // Guarda cada programa en la base de datos
                this._context.Programas.Add(programa);
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }
}
