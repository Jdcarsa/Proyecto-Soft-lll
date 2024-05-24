using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Index(string periodoAcademicoNombre ,string ambienteBusqueda , string docenteBusqueda )
        {
            bool existenDatos = _context.Programas.Any();
            ViewBag.ExistenDatos = existenDatos;

            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h => h.competencia).Include(h =>
            h.docente).Include(h => h.periodoAcademico);

            var periodosAcademicos = await _context.PeriodosAcademicos.Select(p => p.periodoNombre).ToListAsync();
            ViewBag.PeriodosAcademicos = periodosAcademicos;

            if (!string.IsNullOrEmpty(ambienteBusqueda) && !string.IsNullOrEmpty(docenteBusqueda))
            {
                ModelState.AddModelError("", "Buscar solo por el nombre del ambiente o del docente");
                return View();
            }

            if (!string.IsNullOrEmpty(ambienteBusqueda))
            {
                var ambienteExiste = await _context.Ambientes.AnyAsync(a => a.ambienteNombre == ambienteBusqueda);
                if (!ambienteExiste)
                {
                    ModelState.AddModelError("", "El ambiente ingresado no existe.");
                    return View();
                }

                var horarioAmbiente = appDbContext.Where(h => h.ambiente.ambienteNombre == ambienteBusqueda
                && h.periodoAcademico.periodoNombre == periodoAcademicoNombre);
                return View(await horarioAmbiente.ToListAsync());
            }

            if (!string.IsNullOrEmpty(docenteBusqueda))
            {
                var docenteExiste = await _context.Docentes.AnyAsync(d => d.docenteNombre == docenteBusqueda);
                if (!docenteExiste)
                {
                    ModelState.AddModelError("", "El docente ingresado no existe.");
                    return View();
                }

                var horarioDocente = appDbContext.Where(h => h.docente.docenteNombre == docenteBusqueda
                && h.periodoAcademico.periodoNombre == periodoAcademicoNombre);
                return View(await horarioDocente.ToListAsync());
            }

            ViewBag.PeriodoAcademicoNombre = periodoAcademicoNombre;
            ViewBag.AmbienteBusqueda = ambienteBusqueda;
            ViewBag.DocenteBusqueda = docenteBusqueda;

            return View();
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> List(string dia, string hora)
        {

            hora = hora + ":00";
            TimeSpan horaSeleccionada = TimeSpan.Parse(hora);

            var appDbContext = _context.Horarios
            .Include(h => h.ambiente)
            .Include(h => h.competencia)
            .Include(h => h.docente)
            .Include(h => h.periodoAcademico)
            .Where(h => h.horarioDia == dia &&
            (horaSeleccionada >= h.horarioHoraInicio && horaSeleccionada < h.horarioHoraFin));

            return View(await appDbContext.ToListAsync());
        }

        [Authorize(Roles = "Coordinador,Docente")]
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

        [Authorize(Roles = "Coordinador")]
        public IActionResult Create(string dia, int hora)
        {
            int aux = hora + 1;

            var horario = new Horario
            {
                horarioDia = dia,
                horarioHoraInicio = TimeSpan.ParseExact(hora.ToString(), "%h", System.Globalization.CultureInfo.InvariantCulture),
                horarioHoraFin = TimeSpan.ParseExact(aux.ToString(), "%h", System.Globalization.CultureInfo.InvariantCulture),
            };

            horario.horarioDuracion = (int)(horario.horarioHoraFin - horario.horarioHoraInicio).TotalMinutes;
            getAll();
            return View(horario);
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("horarioId,horarioDia,horarioHoraInicio," +
        "horarioHoraFin,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                getAll(horario);
                horario.horarioDuracion = (int)(horario.horarioHoraFin - horario.horarioHoraInicio).TotalHours;
                var validationResult = ValidarHorario(horario);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    getAll(horario);
                    return View(horario);
                }

                _context.Add(horario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(horario);
        }

        private string ValidarHorario(Horario horario)
        {
            var errors = new List<string>();

            if (horario.horarioHoraInicio > horario.horarioHoraFin)
            {
                ModelState.AddModelError("horarioHoraFin", "La hora seleccionada es invalida.");
                errors.Add("horarioHoraFin");
            }

            if (horario.horarioHoraInicio < TimeSpan.FromHours(7) || horario.horarioHoraInicio > TimeSpan.FromHours(21))
            {
                ModelState.AddModelError("horarioHoraInicio", "La hora seleccionada es invalida.");
                errors.Add("horarioHoraInicio");
            }

            if (horario.horarioHoraFin < TimeSpan.FromHours(8) || horario.horarioHoraFin > TimeSpan.FromHours(22))
            {
                ModelState.AddModelError("horarioHoraFin", "La hora seleccionada es invalida.");
                errors.Add("horarioHoraFin");
            }

            if (!_docenteServicio.ValidateDayHours(horario))
            {
                ModelState.AddModelError("docenteId", "El docente excede el limite de horas permitido por dia.");
                errors.Add("docenteId");
            }

            if (!_docenteServicio.ValidateWeekHours(horario))
            {
                ModelState.AddModelError("docenteId", "El docente excede el limite de horas permitido por semana.");
                errors.Add("docenteId");
            }

            if (!_ambienteServicio.IsAvailableSameTime(horario))
            {
                ModelState.AddModelError("ambienteId", "El ambiente no esta dispobile en la franja seleccionada.");
                errors.Add("ambienteId");
            }

            if (!_ambienteServicio.IsAvailable(horario))
            {
                ModelState.AddModelError("ambienteId", "El ambiente no esta disponible en la franja seleccionada.");
                errors.Add("ambienteId");
            }

            if (!_docenteServicio.IsAvailableSameHour(horario))
            {
                ModelState.AddModelError("docenteId", "El docente no esta dispobile en la franja seleccionada.");
                errors.Add("docenteId");
            }

            if (!_docenteServicio.IsAvailable(horario))
            {
                ModelState.AddModelError("docenteId", "El docente no esta dispobile en la franja seleccionada.");
                errors.Add("docenteId");
            }

            return errors.Any() ? string.Join(",", errors) : string.Empty;
        }



        [Authorize(Roles = "Coordinador")]
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
           getAll(horario);
            return View(horario);
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("horarioId,horarioDia,horarioHoraInicio,horarioHoraFin,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
        {
            if (id != horario.horarioId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var validationResult = ValidarHorario(horario);

                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        getAll(horario);
                        return View(horario);
                    }

                    var existingHorario = await _context.Horarios.FindAsync(id);
                    if (existingHorario == null)
                    {
                        return NotFound();
                    }

                    existingHorario.horarioDia = horario.horarioDia;
                    existingHorario.horarioHoraInicio = horario.horarioHoraInicio;
                    existingHorario.horarioHoraFin = horario.horarioHoraFin;
                    existingHorario.horarioDuracion = (int)(horario.horarioHoraFin - horario.horarioHoraInicio).TotalHours;
                    existingHorario.ambienteId = horario.ambienteId;
                    existingHorario.docenteId = horario.docenteId;
                    existingHorario.periodoAcademicoId = horario.periodoAcademicoId;
                    existingHorario.CompetenciaId = horario.CompetenciaId;

                    _context.Update(existingHorario);
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
            getAll(horario);
            return View(horario);
        }


        [Authorize(Roles = "Coordinador")]
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

        [Authorize(Roles = "Coordinador")]
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


        public void getAll()
        {
            ViewData["competenciaId"] = _competenciaServicio.getCompetencias();
            ViewData["docenteId"] = _docenteServicio.getDocentes();
            ViewData["periodoAcademicoId"] = _pAServicio.getPA();
            ViewData["ambienteId"] = _ambienteServicio.getAmbientes();
        }


        public void getAll(Horario horario)
        {
            ViewData["ambienteId"] = _ambienteServicio.getAmbientes(horario.ambienteId);
            ViewData["competenciaId"] = _competenciaServicio.getCompetencias(horario.CompetenciaId);
            ViewData["docenteId"] = _docenteServicio.getDocentes(horario.docenteId);
            ViewData["periodoAcademicoId"] = _pAServicio.getPA(horario.periodoAcademicoId);
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> saveDataProgComp()
        {

            var rutaAlArchivo = "C:\\Users\\ideapad330S\\Documents\\GitHub\\Proyecto-Soft-lll\\ProyectoFinalSoft\\ProyectoFinalSoft\\Json\\Programas.json";


            var contenidoDeArchivo = await System.IO.File.ReadAllTextAsync(rutaAlArchivo);

    
            var rootObject = JsonConvert.DeserializeObject<RootObject>(contenidoDeArchivo);
            var programas = rootObject.programas;

            foreach (var programa in programas)
            {

                this._context.Programas.Add(programa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize (Roles="Docente")]
        public async Task<IActionResult> DocenteHorario(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes.FindAsync(id);


            if(docente == null)
            {
                return NotFound("Docente no existe");
            }

            var appDbContext = _context.Horarios.Include(h => h.ambiente).
                Include(h=> h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);

            var horariosDocente = appDbContext.Where(h => h.docenteId == docente.docenteId);

              if (!horariosDocente.Any())
              {
                        return View();
              }

           return View(await horariosDocente.ToListAsync());
        }
    }
}
