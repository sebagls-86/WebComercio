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
    public class CategoriasController : Controller
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

        public CategoriasController(MyContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,Nombre")] Categoria categoria)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Mensaje"] = "Se ha creado la categoria con éxito";
                    TempData["TipoMensaje"] = 2;
                    _context.Add(categoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["Mensaje"] = "Ha ocurrido un error al crear la cateogoria.";
                    TempData["TipoMensaje"] = 1;
                    return RedirectToAction("Index", "Categorias");

                }
            }
           
            return RedirectToAction("Index", "Categorias");
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,Nombre")] Categoria categoria)
        {
            if (id != categoria.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!CategoriaExists(categoria.CatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["Mensaje"] = "Ha ocurrido un error al editar la categoria.";
                        TempData["TipoMensaje"] = 1;
                        return RedirectToAction("Index", "Categorias");
                    }
                }
                TempData["Mensaje"] = "Se ha editado la categoria exitosamente.";
                TempData["TipoMensaje"] = 2;
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.categorias.FindAsync(id);
            _context.categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.categorias.Any(e => e.CatId == id);
        }
    }
}
