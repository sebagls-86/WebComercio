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

        public async Task<IActionResult> Registrado([Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,TipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Usuario usu = _context.usuarios.Where(u => u.Cuil == usuario.Cuil).FirstOrDefault();

                    if (usu != null)
                    {
                        return RedirectToAction("Index", "Registrar", new { mensaje = "CUIL ya registrado", identificador = 1 });
                    }
                    else
                    {

                        Carro carro = new Carro();

                        _context.Add(usuario);
                        _context.Add(carro);
                        await _context.SaveChangesAsync();

                        Carro lastCarro = _context.carro.OrderBy(c => c.CarroId).Last();
                        usuario.MiCarro = lastCarro.CarroId;
                        Usuario lasUsuario = _context.usuarios.OrderBy(u => u.UsuarioId).Last();
                        carro.UsuarioId = lasUsuario.UsuarioId;
                        _context.usuarios.Update(usuario);
                        _context.carro.Update(carro);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Login", new { mensaje = "¡Usuario correctamente registrado! \n Ya podes iniciar sesión", identificador = 2 });
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
