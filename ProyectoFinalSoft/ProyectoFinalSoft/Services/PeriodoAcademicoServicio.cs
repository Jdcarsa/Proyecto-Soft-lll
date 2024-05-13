using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Services
{
    public class PeriodoAcademicoServicio
    {
        private readonly AppDbContext _context;

        public PeriodoAcademicoServicio(AppDbContext context)
        {
            _context = context;
        }

        public SelectList ObtenerPA()
        {
            var periodoAcademicos = _context.PeriodosAcademicos.Where(d => d.periodoEstado == 1);
            return new SelectList(periodoAcademicos, "periodoId", "periodoNombre");
        }

        public SelectList ObtenerPA(int? horarioPAId)
        {
            var periodoAcademicos = _context.PeriodosAcademicos.Where(d => d.periodoEstado == 1);
            return new SelectList(periodoAcademicos, "periodoId", "periodoNombre", horarioPAId);
        }
    }
}
