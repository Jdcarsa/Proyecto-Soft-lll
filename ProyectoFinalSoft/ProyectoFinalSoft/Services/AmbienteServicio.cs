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

        public SelectList ObtenerAmbientes()
        {
            var ambientes = _context.Ambientes.Where(d => d.ambienteEstado == 1);
            return new SelectList(ambientes, "ambienteId", "infoCompleta");
        }

        public bool EstaDisponible(Horario horario)
        {
            var ambientesDisponibles = _context.Ambientes
                .Where(a => a.ambienteEstado == 1 && a.ambienteId == horario.ambienteId)
                .Where(a => !a.Horarios.Any(h =>
                    (h.horarioHoraInicio == horario.horarioHoraInicio 
                    && h.horarioHoraFin == horario.horarioHoraFin && h.horarioDia == horario.horarioDia)))
                .ToList();

            return ambientesDisponibles.Any();
        }


        public SelectList ObtenerAmbientes(int? horarioAmbienteId)
        {
            var ambientes = _context.Ambientes.Where(d => d.ambienteEstado == 1);
            return new SelectList(ambientes, "ambienteId", "infoCompleta", horarioAmbienteId);
        }
    }


}
