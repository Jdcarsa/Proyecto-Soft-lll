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

        public SelectList ObtenerDocentes()
        {
            var docentes = _context.Docentes.Where(d => d.docenteEstado == 1);
            return new SelectList(docentes, "docenteId", "infoCompleta");
        }

        public SelectList ObtenerDocentes(int? horarioDocenteId)
        {
            var docentes = _context.Docentes.Where(d => d.docenteEstado == 1);
            return new SelectList(docentes, "docenteId", "infoCompleta", horarioDocenteId);
        }

        public bool ValidarHorasTrabajo(Horario horario)
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
                // Docente PT: Maximo 8 horas al dia y 32 a la semana
                return horasDia <= 8 &&
                       horasSemana <= 32;
            }
            else if (docente.docenteTipoContrato == "CNT")
            {
                // Docente CNT: Maximo 10 horas al dia y 40 a la semana
                return horasDia <= 10 &&
                       horasSemana <= 40;
            }

            return true;
        }

        public bool EstaDisponibleMismaHora(Horario horario)
        {
            var docentesDisponibles = _context.Docentes
                .Where(d => d.docenteEstado == 1 && d.docenteId == horario.docenteId)
                .Where(d => !d.Horarios.Any(h =>
                    (h.horarioHoraInicio == horario.horarioHoraInicio
                    && h.horarioHoraFin == horario.horarioHoraFin && h.horarioDia == horario.horarioDia)))
                .ToList();

            return docentesDisponibles.Any();
        }

        public bool EstaDisponible(Horario horario)
        {
            var docentesDisponibles = _context.Docentes
                .Where(d => d.docenteEstado == 1 && d.docenteId == horario.docenteId)
                .Where(d => !d.Horarios.Any(h =>
                    (h.horarioHoraInicio < horario.horarioHoraFin && h.horarioHoraFin > horario.horarioHoraInicio) && h.horarioDia == horario.horarioDia))
                .ToList();

            return docentesDisponibles.Any();
        }

    }
}
