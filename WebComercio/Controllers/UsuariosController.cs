﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebComercio;
using WebComercio.Data;

namespace WebComercio.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MyContext _context;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        public UsuariosController(MyContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public IActionResult Index(string searchString)
        {
            var usuarios = from u in _context.usuarios.Include(u => u.Carro)
                           select u;



            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(u => u.Nombre.Contains(searchString));
            }

            return View(usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create([Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,TipoUsuario")] Usuario usuario)
        {
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Usuario usu = _context.usuarios.Where(u => u.Cuil == usuario.Cuil).FirstOrDefault();

                        if (usu != null)
                        {
                            TempData["Mensaje"] = "Un usuario con CUIL " + usuario.Cuil+ " ya existe.";
                            TempData["TipoMensaje"] = 1;
                            return RedirectToAction("Create", "Usuarios");
                        }
                        else
                        {
                            Carro carro = new Carro();
                            usuario.Carro = carro;
                            usuario.Password = RegistrarController.Encrypt.GetSHA256(usuario.Password);
                            _context.Add(usuario);
                            _context.Add(carro);
                            _context.SaveChanges();

                            Carro lastCarro = _context.carro.OrderBy(c => c.CarroId).Last();
                            usuario.MiCarro = lastCarro.CarroId;
                            _context.usuarios.Update(usuario);
                            _context.carro.Update(carro);
                            _context.SaveChanges();
                            TempData["Mensaje"] = "Se ha creado el usuario " + usuario.Nombre + " exitosamente.";
                            TempData["TipoMensaje"] = 2;
                            return RedirectToAction("Create", "Usuarios");
                        }
                    }
                    catch (Exception)
                    {
                        TempData["Mensaje"] = "Ha ocurrido un error al crear el usuario.";
                        TempData["TipoMensaje"] = 1;
                        return RedirectToAction("Create", "Usuarios");
                    }
                }
                return View(usuario);
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);


        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,MiCarro,TipoUsuario,Bloqueado")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Password = RegistrarController.Encrypt.GetSHA256(usuario.Password);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["Mensaje"] = "No se ha podido editar el usuario.";
                        TempData["TipoMensaje"] = 1;
                        return RedirectToAction("Index", "Usuarios");
                    }
                }
                TempData["Mensaje"] = "Se ha editado el usuario " + usuario.Nombre + " exitosamente.";
                TempData["TipoMensaje"] = 2;
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            _context.usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
