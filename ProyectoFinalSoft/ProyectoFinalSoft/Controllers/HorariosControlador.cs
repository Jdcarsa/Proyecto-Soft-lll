using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h
                => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
            return View(await appDbContext.ToListAsync());
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> List()
        {
            var appDbContext = _context.Horarios.Include(h => h.ambiente).Include(h 
                => h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);
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
        public IActionResult Create()
        {
            obtenerTodos();
            return View();
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("horarioId,horarioDia,horarioHoraInicio," +
            "horarioHoraFin,horarioDuracion,ambienteId,docenteId,periodoAcademicoId,CompetenciaId")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                obtenerTodos(horario);

                var validationResult = ValidarHorario(horario);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    obtenerTodos(horario);
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
            if (horario.horarioHoraInicio > horario.horarioHoraFin)
            {
                ModelState.AddModelError("horarioHoraInicio", "La hora seleccionada es invalida.");
                return "horarioHoraInicio";
            }

            if (horario.horarioHoraInicio < TimeSpan.FromHours(7) || horario.horarioHoraInicio > TimeSpan.FromHours(21))
            {
                ModelState.AddModelError("horarioHoraInicio", "La hora seleccionada es invalida.");
                return "horarioHoraInicio";
            }

            if (horario.horarioHoraFin < TimeSpan.FromHours(8) || horario.horarioHoraFin > TimeSpan.FromHours(22))
            {
                ModelState.AddModelError("horarioHoraFin", "La hora seleccionada es invalida.");
                return "horarioHoraFin";
            }

            if (!_docenteServicio.ValidarHorasTrabajo(horario))
            {
                ModelState.AddModelError("docenteId", "El docente excede el limite de horas permitido por dia o por semana.");
                return "docenteId";
            }

            if (!_ambienteServicio.EstaDisponibleMismaHora(horario))
            {
                ModelState.AddModelError("ambienteId", "El ambiente no esta dispobile en la franja seleccionada.");
                return "ambienteId";
            }

            if (!_ambienteServicio.EstaDisponible(horario))
            {
                ModelState.AddModelError("ambienteId", "El ambiente no esta dispobile en la franja seleccionada.");
                return "ambienteId";
            }

            if (!_docenteServicio.EstaDisponibleMismaHora(horario))
            {
                ModelState.AddModelError("docneteId", "El docente no esta dispobile en la franja seleccionada.");
                return "docneteId";
            }

            if (!_docenteServicio.EstaDisponible(horario))
            {
                ModelState.AddModelError("docneteId", "El docente no esta dispobile en la franja seleccionada.");
                return "docneteId";
            }

            return string.Empty;
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
           obtenerTodos(horario);
            return View(horario);
        }

        [Authorize(Roles = "Coordinador")]
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

                    var validationResult = ValidarHorario(horario);

                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        obtenerTodos(horario);
                        return View(horario);
                    }

                    _context.Horarios.Remove(horario);
                    await _context.SaveChangesAsync();
                    _context.Horarios.Add(horario);
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

        [Authorize(Roles = "Coordinador")]
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

        
        [Authorize (Roles="Docente")]
        public async Task<IActionResult> DocenteHorario(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.usuarioId == id);

            if(usuario == null)
            {
                return NotFound();
            }

            var appDbContext = _context.Horarios.Include(h => h.ambiente).
                Include(h=> h.competencia).Include(h => h.docente).Include(h => h.periodoAcademico);

            var horariosDocente = appDbContext.Where(h => h.docenteId == usuario.docenteId);

          if (!horariosDocente.Any())
          {
                    return NotFound();
           }

           return View(await horariosDocente.ToListAsync());
        }
    }
}
