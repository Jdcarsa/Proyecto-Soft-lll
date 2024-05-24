using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    [Authorize(Roles = "Coordinador")]
    public class AmbientesControlador : Controller
    {
        private readonly AppDbContext _context;

        public AmbientesControlador(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string ambienteBusqueda)
        {

            var ambientes = from ambiente in _context.Ambientes select ambiente;


            if (!String.IsNullOrEmpty(ambienteBusqueda))
            {
                ambientes = ambientes.Where(amb => (amb.ambienteNombre + " " + amb.ambienteUbicacion)
                .Contains(ambienteBusqueda));
            }

            ambientes = ambientes.OrderBy(ambientes => ambientes.ambienteCodigo);

            return View(await ambientes.ToListAsync());
        }


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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ambienteId,ambienteCodigo,ambienteNombre,ambienteUbicacion,ambienteTipo,ambienteCapacidad,ambienteEstado")] Ambiente ambiente)
        {
            if (ModelState.IsValid)
            {

                var existeAmbiente = await _context.Ambientes.AnyAsync(amb => amb.ambienteCodigo == ambiente.ambienteCodigo);
                if (existeAmbiente)
                {
                    ModelState.AddModelError("ambienteCodigo", "El código ingresado ya existe.");
                    return View(ambiente);
                }
                ambiente.ambienteEstado = 1;
                _context.Add(ambiente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ambiente);
        }

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
                var existeAmbiente = await _context.Ambientes.AnyAsync(amb => amb.ambienteCodigo == ambiente.ambienteCodigo && amb.ambienteId != ambiente.ambienteId);
                if (existeAmbiente)
                {
                    ModelState.AddModelError("ambienteCodigo", "El código ingresado ya existe.");
                    return View(ambiente);
                }

                try
                {
                    ambiente.ambienteEstado = 1;
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
