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
            return new SelectList(ambientes, "ambienteId", "ambienteCodigo");
        }

        public SelectList ObtenerAmbientes(int? horarioAmbienteId)
        {
            var ambientes = _context.Ambientes.Where(d => d.ambienteEstado == 1);
            return new SelectList(ambientes, "ambienteId", "ambienteCodigo", horarioAmbienteId);
        }
    }


}
