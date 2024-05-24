using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Services
{
    public class AmbienteServicio
    {

        private readonly AppDbContext _context;

        public AmbienteServicio(AppDbContext context)
        {
            _context = context;
        }

        public SelectList getAmbientes()
        {
            var ambientes = _context.Ambientes.Where(d => d.ambienteEstado == 1);
            return new SelectList(ambientes, "ambienteId", "infoCompleta");
        }

        public bool IsAvailableSameTime(Horario horario)
        {
            var ambientesDisponibles = _context.Ambientes
                .Where(a => a.ambienteEstado == 1 && a.ambienteId == horario.ambienteId)
                .Where(a => !a.Horarios.Any(h =>
                    h.horarioId != horario.horarioId &&
                    h.horarioHoraInicio == horario.horarioHoraInicio
                    && h.horarioHoraFin == horario.horarioHoraFin
                    && h.horarioDia == horario.horarioDia && h.periodoAcademicoId == horario.periodoAcademicoId))
                .ToList();

            return ambientesDisponibles.Any();
        }

        public bool IsAvailable(Horario horario)
        {
            var ambientesDisponibles = _context.Ambientes
                .Where(a => a.ambienteEstado == 1 && a.ambienteId == horario.ambienteId)
                .Where(a => !a.Horarios.Any(h =>
                    h.horarioId != horario.horarioId &&
                    h.horarioHoraInicio < horario.horarioHoraFin
                    && h.horarioHoraFin > horario.horarioHoraInicio
                    && h.horarioDia == horario.horarioDia && h.periodoAcademicoId == horario.periodoAcademicoId))
                .ToList();

            return ambientesDisponibles.Any();
        }


        public SelectList getAmbientes(int? horarioAmbienteId)
        {
            var ambientes = _context.Ambientes.Where(d => d.ambienteEstado == 1);
            return new SelectList(ambientes, "ambienteId", "infoCompleta", horarioAmbienteId);
        }
    }


}
