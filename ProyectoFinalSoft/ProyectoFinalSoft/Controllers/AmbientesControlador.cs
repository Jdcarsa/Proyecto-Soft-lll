using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalSoftwarelll.Models;
using FinalSoftwarelll.Services;

namespace FinalSoftwarelll.Controllers
{
    public class AmbientesControlador : Controller
    {
        private readonly AppDbContext _context;

        public AmbientesControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: AmbientesControlador
        public async Task<IActionResult> Index(string ambienteBusqueda)
        {
            // Obtener todos los ambientes
            var ambientes = from ambiente in _context.Ambientes select ambiente;

            // Verificar si se proporcionó un término de búsqueda
            if (!String.IsNullOrEmpty(ambienteBusqueda))
            {
                // Filtrar los ambientes cuyo nombre o ubicación contengan el término de búsqueda
                ambientes = ambientes.Where(amb => (amb.ambienteNombre + " " + amb.ambienteUbicacion).Contains(ambienteBusqueda));
            }

            // Devolver la vista con los resultados filtrados
            return View(await ambientes.ToListAsync());
        }


        // GET: AmbientesControlador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes
                .FirstOrDefaultAsync(m => m.ambienteId == id);
            if (ambiente == null)
            {
                return NotFound();
            }

            return View(ambiente);
        }

        // GET: AmbientesControlador/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AmbientesControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ambienteId,ambienteCodigo,ambienteNombre,ambienteUbicacion,ambienteTipo,ambienteCapacidad,ambienteEstado")] Ambiente ambiente)
        {
            if (ModelState.IsValid)
            {

                var existeAmbiente = await _context.Ambientes.AnyAsync(amb => amb.ambienteCodigo == ambiente.ambienteCodigo);
                if (existeAmbiente)
                {
                    ModelState.AddModelError("ambienteCodigo", "El ID ingresado ya existe.");
                    return View(ambiente);
                }
                ambiente.ambienteEstado = 1;
                _context.Add(ambiente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ambiente);
        }

        // GET: AmbientesControlador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes.FindAsync(id);
            if (ambiente == null)
            {
                return NotFound();
            }
            return View(ambiente);
        }

        // POST: AmbientesControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ambienteId,ambienteCodigo,ambienteNombre,ambienteUbicacion,ambienteTipo,ambienteCapacidad,ambienteEstado")] Ambiente ambiente)
        {
            if (id != ambiente.ambienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ambiente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmbienteExists(ambiente.ambienteId))
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
            return View(ambiente);
        }

        // GET: AmbientesControlador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await _context.Ambientes
                .FirstOrDefaultAsync(m => m.ambienteId == id);
            if (ambiente == null)
            {
                return NotFound();
            }

            return View(ambiente);
        }

        // POST: AmbientesControlador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ambiente = await _context.Ambientes.FindAsync(id);
            if (ambiente != null)
            {
                ambiente.ambienteEstado = ambiente.ambienteEstado == 0 ? 1 : 0;
                _context.Ambientes.Update(ambiente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmbienteExists(int id)
        {
            return _context.Ambientes.Any(e => e.ambienteId == id);
        }
    }
}
