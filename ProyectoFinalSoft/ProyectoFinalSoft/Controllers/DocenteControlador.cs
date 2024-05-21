using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Index(string parametroBusqueda)
        {

            var docentes = from docente in _context.Docentes
                           select docente;

            if (!String.IsNullOrEmpty(parametroBusqueda))
            {
                docentes = docentes.Where(d => (d.docenteNombre + " " + d.docenteApellido)!.Contains(parametroBusqueda));
            }

            docentes = docentes.OrderBy(d => d.docenteNombre);

            return View(await docentes.ToListAsync());

        }

        [Authorize(Roles = "Coordinador,Docente")]
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

        [Authorize(Roles = "Coordinador")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("docenteId,docenteNombre,docenteApellido,docenteTipoId,docenteNumId,docenteTipo,docenteTipoContrato,docenteArea,docenteEstado")] Docente docente)
        {

            var existeNumIdDocente = await _context.Docentes.AnyAsync(d => d.docenteNumId == docente.docenteNumId);
            if (existeNumIdDocente)
            {
                ModelState.AddModelError("docenteNumId", "El número de identificación ya existe.");
                return View(docente);
            }
            if (ModelState.IsValid)
            {
                docente.docenteEstado = 1;
                _context.Docentes.Add(docente);
                await _context.SaveChangesAsync();
                await CreateUser(docente);
                //await CreateUser();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        public async Task<IActionResult> CreateUser(Docente docente)
        {
            try
            {
                var passwordHasher = new PasswordHasher<object>();

                Usuario user = new Usuario();
                user.docenteId = docente.docenteId;
                user.docente = docente;
                user.usuarioEstado = 1;
                user.usuarioLogin = GenerarNombreUsuario(docente.docenteNombre, docente.docenteApellido);
                user.usuarioRol = 1;
                
                string contrasenaGenerada = GenerarContrasena();

                string path = "C:\\Users\\ideapad330S\\Documents\\U\\usercontra.txt";
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine($"Usuario: {user.usuarioLogin}, Contraseña: {contrasenaGenerada}");
                }

                user.usuarioPassword = passwordHasher.HashPassword(null, contrasenaGenerada);
                _context.Usuarios.Add(user);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un error al crear el usuario", error = ex.Message });
            }
        }


        public async Task<IActionResult> CreateUser()
        {
            try
            {
                var passwordHasher = new PasswordHasher<object>();

                Usuario user = new Usuario();
                user.coordinadorId = 1;
                user.usuarioEstado = 1;
                user.usuarioLogin = GenerarNombreUsuario("Jose", "coor");
                user.usuarioRol = 0;

                string contrasenaGenerada = GenerarContrasena();

                string path = "C:\\Users\\ideapad330S\\Documents\\U\\usercontra.txt";
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine($"Usuario: {user.usuarioLogin}, Contraseña: {contrasenaGenerada}");
                }

                user.usuarioPassword = passwordHasher.HashPassword(null, contrasenaGenerada);
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


        [Authorize(Roles = "Coordinador")]
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

        [Authorize(Roles = "Coordinador")]
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

        [Authorize(Roles = "Coordinador")]
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

        [Authorize(Roles = "Coordinador")]
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
