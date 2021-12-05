using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebComercio.Data;

namespace WebComercio.Controllers
{
    public class LoginController : Controller
    {

        private readonly MyContext _context;

        public LoginController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index(string mensaje, int identificador)
        {
            ViewBag.Mensaje = mensaje;

            ViewBag.Identificador = identificador;
            return View();
        }

        public IActionResult Login([Bind("UsuarioId, Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Usuario loginUsuario = _context.usuarios.Where(u => u.UsuarioId == usuario.UsuarioId && u.Password == usuario.Password).FirstOrDefault();

                    if (usuario == null)
                    {
                        return RedirectToAction("Index", "Login", new { mensaje = "Usuario o contraseña incorrectos", identificador = 3 });
                    }
                    else if (loginUsuario.TipoUsuario == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login", new { mensaje = "Usuario NO administrador", identificador = 4 });
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Registrar", new { mensaje = "Error", identificador = 1 });
                }

            }
            return View(usuario);
        }
    }
}


