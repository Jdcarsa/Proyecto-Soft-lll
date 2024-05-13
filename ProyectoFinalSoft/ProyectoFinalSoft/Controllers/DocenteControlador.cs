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
        public async Task<IActionResult> Index(string parametroBusqueda)
        {

            var docentes = from docente in _context.Docentes select docente;

            if (!String.IsNullOrEmpty(parametroBusqueda))
            {
                docentes = docentes.Where(d => (d.docenteNombre + " " + d.docenteApellido)!.Contains(parametroBusqueda));
            }
            return View(await docentes.ToListAsync());
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
                await CreateUser(docente);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        public async Task<IActionResult> CreateUser(Docente docente)
        {
            try
            {
                Usuario user = new Usuario();
                user.docenteId = docente.docenteId;
                user.docente = docente;
                user.usuarioEstado = 1;
                user.usuarioLogin = GenerarNombreUsuario(docente.docenteNombre, docente.docenteApellido);
                user.usuarioPassword = GenerarContrasena();
                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un error al crear el usuario", error = ex.Message });
            }
        }

        private string GenerarNombreUsuario(string? nombre, string? apellido)
        {
            int i = 1;
            string nombreUsuarioBase = nombre?.Substring(0, 3) + apellido?.Substring(0, 3);
            string nombreUsuario = nombreUsuarioBase;
            while (_context.Usuarios.Any(u => u.usuarioLogin == nombreUsuario))
            {
                nombreUsuario = nombreUsuarioBase + i.ToString();
                i++;
            }
            return nombreUsuario;
        }

        private string GenerarContrasena()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
                    docente.docenteEstado = 1;
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
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
                docente.docenteEstado = docente.docenteEstado == 0 ? 1 : 0;
                _context.Docentes.Update(docente);
            }
            DisableUser(docente.docenteId, docente.docenteEstado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void DisableUser(int docenteId, int docenteEstado)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.docenteId == docenteId);
            if (usuario != null)
            {
                usuario.usuarioEstado = docenteEstado == 1 ? 1 : 0;
                _context.Usuarios.Update(usuario);
            }
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.docenteId == id);
        }


    }
}
