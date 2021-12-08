﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebComercio.Data;

namespace WebComercio.Controllers
{
    public class MercadoController : Controller
    {
        private readonly MyContext _context;

        public MercadoController(MyContext context)
        {
            _context = context;
        }


        public async Task <IActionResult> Index(string mensaje, int identificador, string searchString, string orderByName, string orderByPrice, string orderByNameDesc, string orderByPriceDesc, string Cat, string sortOrder)

        {
            List<Producto> productosOrdenados = new List<Producto>();
            productosOrdenados = _context.productos.OrderBy(producto => producto.ProductoId).ToList();
            List<Categoria> categorias = new List<Categoria>();
            categorias = _context.categorias.ToList();

            ViewBag.Productos = productosOrdenados;
            ViewBag.categorias = categorias;
            ViewBag.Mensaje = mensaje;

            ViewBag.Identificador = identificador;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["AmountSortParm"] = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
            ViewData["CurrentFilter"] = searchString;
            ViewData["ByName"] = orderByName;
            ViewData["ByPrice"] = orderByPrice;
            ViewData["ByNameDesc"] = orderByNameDesc;
            ViewData["ByPriceDesc"] = orderByPriceDesc;


            var productos = from p in _context.productos.Include(p => p.Cat)
                            select p;


            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(u => u.Nombre.Contains(searchString));

                if (orderByName == "on" && orderByPrice == "on")
                {
                    productos = productos.OrderBy(p => p.Nombre).ThenBy(p => p.Precio);
                }

                else if (orderByNameDesc == "on" && orderByPriceDesc == "on")
                {
                    productos = productos.OrderByDescending(p => p.Nombre).ThenByDescending(p => p.Precio);
                }
                else if (orderByNameDesc == "on" && orderByPrice == "on")
                {
                    productos = productos.OrderByDescending(p => p.Nombre).ThenBy(p => p.Precio);
                }

                else if (orderByName == "on" && orderByPriceDesc == "on")
                {
                    productos = productos.OrderBy(p => p.Nombre).ThenByDescending(p => p.Precio);
                }

                else if (orderByPrice == "on")
                {
                    productos = productos.OrderBy(p => p.Precio);
                }
                else if (orderByName == "on")
                {
                    productos = productos.OrderBy(p => p.Nombre);
                }

                else if (orderByPriceDesc == "on")
                {
                    productos = productos.OrderByDescending(p => p.Precio);
                }
                else if (orderByNameDesc == "on")
                {
                    productos = productos.OrderByDescending(p => p.Nombre);
                }

                ViewBag.Productos = productos;

            }



            switch (sortOrder)
            {
                case "name_desc":
                    productos = productos.OrderByDescending(p => p.Nombre).ThenByDescending(p => p.Precio);
                    ViewBag.Productos = productos;
                    break;
                case "Price":
                    productos = productos.OrderBy(p => p.Precio);
                    ViewBag.Productos = productos;
                    break;
                case "price_desc":
                    ViewBag.Productos = productos.OrderByDescending(p => p.Precio).ThenBy(p => p.Nombre);
                    Console.WriteLine(productos);
                    break;
                case "Amount":
                    productos = productos.OrderBy(p => p.Cantidad);
                    ViewBag.Productos = productos;
                    break;
                case "amount_desc":
                    productos = productos.OrderByDescending(p => p.Cantidad);
                    ViewBag.Productos = productos;
                    break;
                case "Category":
                    productos = productos.OrderBy(p => p.Cat.Nombre);
                    ViewBag.Productos = productos;
                    break;
                case "category_desc":
                    productos = productos.OrderByDescending(p => p.Cat.Nombre);
                    ViewBag.Productos = productos;
                    break;
                
            }


            if (!String.IsNullOrEmpty(Cat))
            {
                productos = productos.Where(u => u.Cat.Nombre.Contains(Cat));
                ViewBag.Productos = productos;
            }


            return View(await productos.ToListAsync());
        }

        //detalle producto
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos.Include(p => p.Cat).FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public async Task<IActionResult> Carro()
        {



            //var Carro_productos = await _context.Carro_productos.Include(p => p.Producto).FirstOrDefaultAsync(m => m.Id_Carro == id);
            var Carro_productos = await _context.Carro_productos.Include(p => p.Producto).Include(c => c.Carro).ToListAsync();

            if (Carro_productos == null)
            {
                return NotFound();
            }
            ViewBag.Carroproductos = Carro_productos;
            return View(Carro_productos);
        }


        public  IActionResult AgregarProducto(int ProductoId, int Cantidad)
        {
            if (ModelState.IsValid)
            {


                int usuarioID = 4;

                Usuario usuarioEncontrado =  _context.usuarios.Include(c => c.Carro).FirstOrDefault(u => u.UsuarioId == usuarioID);
                Producto productoEncontrado =  _context.productos.Include(c => c.Carro_productos).FirstOrDefault(p => p.ProductoId == ProductoId);


                Carro cart = usuarioEncontrado.Carro;


                //Producto productoCarro = cart.Carro_productos.FirstOrDefault(p => p.Id_Producto == productoEncontrado.ProductoId);
                bool estaEnCarro = false;

                foreach (Producto prod in cart.ProductosCompra)
                {
                    foreach (Carro_productos cp in cart.Carro_productos)
                    {
                        if (cp.Id_Carro == cart.CarroId && cp.Id_Producto == prod.ProductoId)
                        {
                            cp.Cantidad += Cantidad;
                            _context.carro.Update(cart);
                            _context.SaveChanges();
                            estaEnCarro = true;
                        }
                    }
                }


                if (!estaEnCarro)
                {
                    cart.ProductosCompra.Add(productoEncontrado);
                    _context.carro.Update(cart);
                    _context.SaveChanges();

                    cart.Carro_productos.Last<Carro_productos>().Cantidad = Cantidad;
                    _context.carro.Update(cart);
                    _context.SaveChanges();
                }
                
                
                

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MisDatos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios.FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public async Task<IActionResult> EditData(int? id)
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
        public async Task<IActionResult> EditData(int id, string nueva1, string nueva2, [Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,MiCarro,TipoUsuario")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (nueva1 != null)
                    {

                        if (nueva1 == nueva2)
                        {
                            usuario.Password = nueva2;
                        }
                        else
                        {
                            ViewBag.Mensaje = "Las contraseñas deben coincidir";
                        }
                    }
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
