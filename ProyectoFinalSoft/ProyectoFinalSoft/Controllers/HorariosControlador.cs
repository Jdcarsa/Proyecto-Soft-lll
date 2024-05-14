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
        private readonly AmbienteServicio _ambienteServicio;
        private readonly CompetenciaServicio _competenciaServicio;
        private readonly PeriodoAcademicoServicio _pAServicio;

        public HorariosControlador(AppDbContext context, DocenteServicio docenteServicio,
            AmbienteServicio ambienteServicio, CompetenciaServicio competenciaServicio, 
            PeriodoAcademicoServicio pAServicio)
        {
            _context = context;
            _docenteServicio = docenteServicio;
            _ambienteServicio = ambienteServicio;
            _competenciaServicio = competenciaServicio;
            _pAServicio = pAServicio;
        }

        // GET: HorariosControlador
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h
                => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h 
                => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
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
            obtenerTodos();
            return View();
        }

        // POST: HorariosControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("horarioId,horarioDia,horarioHoraInicio," +
            "horarioHoraFin,horarioDuracion,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                
                if (!ValidarHorasTrabajo(horario))
                {
                    ModelState.AddModelError("", "El docente excede el limite de horas permitido por dia o por semana.");
                    obtenerTodos(horario);
                    return View(horario);
                }

                _context.Add(horario);
                await _context.SaveChangesAsync();
                obtenerTodos(horario);
                return RedirectToAction(nameof(Index));
            }
            
            return View(horario);
        }

        private bool ValidarHorasTrabajo(Horario horario)
        {
            var docente = _context.Docentes.FirstOrDefault(d => d.docenteId == horario.docenteId);

            if (docente == null)
            {
                return true;
            }

            var franjasSemana = _context.Horarios
                .Where(h => h.docenteId == horario.docenteId &&
                            (h.horarioDia == "Lunes" || h.horarioDia == "Martes" || h.horarioDia == "Miércoles" ||
                             h.horarioDia == "Jueves" || h.horarioDia == "Viernes" || h.horarioDia == "Sábado"))
                .ToList();

            // Calcular las horas trabajadas por el docente en el día
            var horasDia = franjasSemana.Where(h => h.horarioDia == horario.horarioDia)
                                         .Sum(h => h.horarioDuracion);
         
            // Calcular las horas trabajadas por el docente en la semana
            var horasSemana = franjasSemana.Sum(h => h.horarioDuracion);

            horasDia = horasDia + horario.horarioDuracion;
            horasSemana = horasSemana + horario.horarioDuracion;

            if (docente.docenteTipoContrato == "PT")
            {
                // Docente CNT: Máximo 8 horas al día y 32 a la semana
                return horasDia  <= 8 &&
                       horasSemana <= 32;
            }
            else if (docente.docenteTipoContrato == "CNT")
            {
                // Docente CNT: Máximo 10 horas al día y 40 a la semana
                return horasDia  <= 10 &&
                       horasSemana <= 40;
            }

   
            return true;
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
                    if (!ValidarHorasTrabajo(horario))
                    {
                        ModelState.AddModelError("", "El docente excede el limite de horas permitido por dia o por semana.");
                        obtenerTodos(horario);
                        return View(horario);
                    }
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


        public void obtenerTodos()
        {
            ViewData["competenciaId"] = _competenciaServicio.ObtenerCompetencias();
            ViewData["docenteId"] = _docenteServicio.ObtenerDocentes();
            ViewData["periodoAcademicoId"] = _pAServicio.ObtenerPA();
            ViewData["ambienteId"] = _ambienteServicio.ObtenerAmbientes();
        }


        public void obtenerTodos(Horario horario)
        {
            ViewData["ambienteId"] = _ambienteServicio.ObtenerAmbientes(horario.ambienteId);
            ViewData["competenciaId"] = _competenciaServicio.ObtenerCompetencias(horario.CompetenciaId);
            ViewData["docenteId"] = _docenteServicio.ObtenerDocentes(horario.docenteId);
            ViewData["periodoAcademicoId"] = _pAServicio.ObtenerPA(horario.periodoAcademicoId);
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
