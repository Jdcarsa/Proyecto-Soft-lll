using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Services
{
    public class DocenteServicio
    {
        private readonly AppDbContext _context;

        public DocenteServicio(AppDbContext context)
        {
            _context = context;
        }

        public SelectList getDocentes()
        {
            var docentes = _context.Docentes.Where(d => d.docenteEstado == 1);
            return new SelectList(docentes, "docenteId", "infoCompleta");
        }

        public SelectList getDocentes(int? horarioDocenteId)
        {
            var docentes = _context.Docentes.Where(d => d.docenteEstado == 1);
            return new SelectList(docentes, "docenteId", "infoCompleta", horarioDocenteId);
        }


        public bool ValidateDayHours(Horario horario)
        {
            if (horario == null)
            {
                throw new ArgumentNullException(nameof(horario));
            }

            var docente = _context.Docentes.FirstOrDefault(d => d.docenteId == horario.docenteId);

            if (docente == null)
            {
                return true;
            }

            var franjasDia = _context.Horarios
                .Where(h => h.docenteId == horario.docenteId && h.periodoAcademicoId 
                == horario.periodoAcademicoId && h.horarioDia == horario.horarioDia)
                .ToList();

            var horasDia = franjasDia.Sum(h => h.horarioDuracion) + horario.horarioDuracion;

            if (docente.docenteTipoContrato == "PT")
            {
                // Docente PT: Máximo 8 horas al día
                return horasDia <= 8;
            }
            else if (docente.docenteTipoContrato == "CNT")
            {
                // Docente CNT: Máximo 10 horas al día
                return horasDia <= 10;
            }

            return true;
        }

        public bool ValidateWeekHours(Horario horario)
        {
            if (horario == null)
            {
                throw new ArgumentNullException(nameof(horario));
            }

            var docente = _context.Docentes.FirstOrDefault(d => d.docenteId == horario.docenteId);

            if (docente == null)
            {
                return true;
            }

            var franjasSemana = _context.Horarios
                .Where(h => h.docenteId == horario.docenteId && h.periodoAcademicoId == horario.periodoAcademicoId &&
                            (h.horarioDia == "Lunes" || h.horarioDia == "Martes" || h.horarioDia == "Miercoles" ||
                             h.horarioDia == "Jueves" || h.horarioDia == "Viernes" || h.horarioDia == "Sabado"))
                .ToList();

            var horasSemana = franjasSemana.Sum(h => h.horarioDuracion) + horario.horarioDuracion;

            if (docente.docenteTipoContrato == "PT")
            {
                // Docente PT: Máximo 32 horas a la semana
                return horasSemana <= 32;
            }
            else if (docente.docenteTipoContrato == "CNT")
            {
                // Docente CNT: Máximo 40 horas a la semana
                return horasSemana <= 40;
            }

            return true;
        }


        public bool IsAvailableSameHour(Horario horario)
        {
            var docentesDisponibles = _context.Docentes
                .Where(d => d.docenteEstado == 1 && d.docenteId == horario.docenteId)
                .Where(d => !d.Horarios.Any(h =>
                    h.horarioId != horario.horarioId &&
                    h.horarioHoraInicio == horario.horarioHoraInicio
                    && h.horarioHoraFin == horario.horarioHoraFin
                    && h.horarioDia == horario.horarioDia))
                .ToList();

            return docentesDisponibles.Any();
        }


        public bool IsAvailable(Horario horario)
        {
            var docentesDisponibles = _context.Docentes
                .Where(d => d.docenteEstado == 1 && d.docenteId == horario.docenteId)
                .Where(d => !d.Horarios.Any(h =>
                    h.horarioId != horario.horarioId &&
                    h.horarioHoraInicio < horario.horarioHoraFin
                    && h.horarioHoraFin > horario.horarioHoraInicio
                    && h.horarioDia == horario.horarioDia))
                .ToList();

            return docentesDisponibles.Any();
        }


    }
}
