using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalSoft.Models;
using ProyectoFinalSoft.Models.LoginModel;
using ProyectoFinalSoft.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProyectoFinalSoft.Controllers
{
    public class AccesoControlador : Controller
    {
        private readonly AppDbContext _context;

        public AccesoControlador(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (User.IsInRole("Coordinador"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (User.IsInRole("Docente"))
                {
                    return RedirectToAction("IndexDocente", "Home");
                }
            }
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("usuario", "contrasenia")] LoginUsuario login)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.usuarioLogin == login.usuario);
            if (user == null)
            {
                ViewData["Message"] = "Usuario o contraseña incorrectos";
                return View();
            }

            var passwordHasher = new PasswordHasher<object>();
            var resultado = passwordHasher.VerifyHashedPassword(null, user.usuarioPassword, login.contrasenia);

            if (resultado != PasswordVerificationResult.Success)
            {
                ViewData["Message"] = "Usuario o contraseña incorrectos";
                return View();
            }

            var name = user.usuarioRol == 0 ? user.coordinadorId.ToString() : user.docenteId.ToString();
            var rol = user.usuarioRol == 0 ? "Coordinador" : "Docente";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties { AllowRefresh = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction(user.usuarioRol == 0 ? "Index" : "IndexDocente", "Home");
        }

    }
}
