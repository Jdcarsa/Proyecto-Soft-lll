using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Services;

namespace ProyectoFinalSoft.Controllers
{
    public class DocenteControlador : Controller
    {
        private readonly AppDbContext _context;

        public DocenteControlador(AppDbContext context)
        {
            _context = context;
        }

        // GET: DocenteControlador
        public async Task<IActionResult> Index()
        {
            return View(await _context.Docentes.ToListAsync());
        }

        // GET: DocenteControlador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(m => m.docenteId == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // GET: DocenteControlador/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocenteControlador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("docenteId,docenteNombre,docenteApellido,docenteTipoId,docenteNumId,docenteTipo,docenteTipoContrato,docenteArea,docenteEstado")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                docente.docenteEstado = 1;
                _context.Docentes.Add(docente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(docente);
        }

        public async Task<IActionResult> CreateUser(string nombre , string apellido)
        {

           // _context.Usuarios.Add();
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: DocenteControlador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }
            return View(docente);
        }

        // POST: DocenteControlador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("docenteId,docenteNombre,docenteApellido,docenteTipoId,docenteNumId,docenteTipo,docenteTipoContrato,docenteArea,docenteEstado")] Docente docente)
        {
            if (id != docente.docenteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocenteExists(docente.docenteId))
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
            return View(docente);
        }

        // GET: DocenteControlador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .FirstOrDefaultAsync(m => m.docenteId == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // POST: DocenteControlador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.docenteId == id);
        }
    }
}
