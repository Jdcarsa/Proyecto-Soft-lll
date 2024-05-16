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
            if(User.Identity!.IsAuthenticated) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("usuario","contrasenia")] LoginUsuario login)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.usuarioLogin == login.usuario);
            if (user == null)
            {
                ViewData["Message"] = "Usuario o contraseña incorrectos";
                return View();
            }

            var passwordHasher = new PasswordHasher<object>();
            var resultado = passwordHasher.VerifyHashedPassword(null, user.usuarioPassword, login.contrasenia);

            if (resultado == PasswordVerificationResult.Success)
            {
                List<Claim> claims;
                if (user.usuarioRol == 0)
                {
                     claims = new List<Claim>()
                    {
                      new Claim(ClaimTypes.Name,user.usuarioLogin),
                      new Claim(ClaimTypes.Role, "Coordinador"),
                    };
                } else 
                {
                    claims = new List<Claim>()
                     {
                       new Claim(ClaimTypes.Name,user.usuarioLogin),
                       new Claim(ClaimTypes.Role, "Docente"),
                    };
                }


                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );
                
                if (user.usuarioRol == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (user.usuarioRol == 1)
                {
                    return RedirectToAction("Privacy", "Home");
                }
            }

            ViewData["Message"] = "Usuario o contraseña incorrectos";
            return View();
            
        }
    }
}
