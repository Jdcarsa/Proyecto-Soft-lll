using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            return new SelectList(_context.Docentes, "docenteId", "infoCompleta");
        }
    }
}
