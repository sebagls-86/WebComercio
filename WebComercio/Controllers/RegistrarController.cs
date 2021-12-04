using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebComercio.Data;

namespace WebComercio.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly MyContext _context;

        public RegistrarController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Registrado([Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,TipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                Carro carro = new Carro();
                usuario.TipoUsuario = 1;
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

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
    }
}
