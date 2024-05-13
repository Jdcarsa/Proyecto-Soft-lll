using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Services
{
    public class CompetenciaServicio
    {

        private readonly AppDbContext _context;

        public CompetenciaServicio(AppDbContext context)
        {
            _context = context;
        }

        public SelectList ObtenerCompetencias()
        {
            var competencias = _context.Competencias.Where(d => d.competenciaEstado == 1);
            return new SelectList(competencias, "competenciaId", "competenciaNombre");
        }

        public SelectList ObtenerCompetencias(int? horarioCompetenciaId)
        {
            var competencias = _context.Competencias.Where(d => d.competenciaEstado == 1);
            return new SelectList(_context.Competencias, "competenciaId", "competenciaNombre", horarioCompetenciaId);
        }
    }
}
