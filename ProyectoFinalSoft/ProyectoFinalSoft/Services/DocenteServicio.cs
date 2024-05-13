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

    }
}
