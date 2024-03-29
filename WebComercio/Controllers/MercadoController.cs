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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }


        public async Task<IActionResult> Index(string mensaje, int identificador, string searchString, string orderByName, string orderByPrice, string Cat, string sortOrder)

        {
            List<Producto> productosOrdenados = new List<Producto>();
            productosOrdenados = _context.productos.Where(producto => producto.Cantidad > 0).OrderBy(producto => producto.ProductoId).ToList();
            List<Categoria> categorias = new List<Categoria>();
            categorias = _context.categorias.ToList();
            ProductosEnCarro(identificador);
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



            var productos = from p in _context.productos.Include(p => p.Cat)
                            where p.Cantidad > 0
                            select p;


            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(u => u.Nombre.Contains(searchString));

                if (orderByName == "1" && orderByPrice == "1")
                {
                    productos = productos.OrderBy(p => p.Nombre).ThenBy(p => p.Precio);
                }

                else if (orderByName == "-1" && orderByPrice == "-1")
                {
                    productos = productos.OrderByDescending(p => p.Nombre).ThenByDescending(p => p.Precio);
                }
                else if (orderByName == "-1" && orderByPrice == "1")
                {
                    productos = productos.OrderByDescending(p => p.Nombre).ThenBy(p => p.Precio);
                }

                else if (orderByName == "1" && orderByPrice == "-1")
                {
                    productos = productos.OrderBy(p => p.Nombre).ThenByDescending(p => p.Precio);
                }

                else if (orderByPrice == "1")
                {
                    productos = productos.OrderBy(p => p.Precio);
                }
                else if (orderByName == "1")
                {
                    productos = productos.OrderBy(p => p.Nombre);
                }

                else if (orderByPrice == "-1")
                {
                    productos = productos.OrderByDescending(p => p.Precio);
                }
                else if (orderByName == "-1")
                {
                    productos = productos.OrderByDescending(p => p.Nombre);
                }

            }



            switch (sortOrder)
            {
                case "name_desc":
                    productos = productos.OrderByDescending(p => p.Nombre).ThenByDescending(p => p.Precio);
                    break;
                case "Price":
                    productos = productos.OrderBy(p => p.Precio);
                    break;
                case "price_desc":
                    productos.OrderByDescending(p => p.Precio).ThenBy(p => p.Nombre);
                    break;
                case "Amount":
                    productos = productos.OrderBy(p => p.Cantidad);
                    break;
                case "amount_desc":
                    productos = productos.OrderByDescending(p => p.Cantidad);
                    break;
                case "Category":
                    productos = productos.OrderBy(p => p.Cat.Nombre);
                    break;
                case "category_desc":
                    productos = productos.OrderByDescending(p => p.Cat.Nombre);
                    break;

            }


            if (!String.IsNullOrEmpty(Cat))
            {
                productos = productos.Where(u => u.Cat.Nombre.Contains(Cat));
            }
            ViewBag.identificador = identificador;

            return View(await productos.ToListAsync());
        }

        //detalle producto
        public async Task<IActionResult> Details(int id, int identificador)
        {

            ViewBag.identificador = identificador;
            ProductosEnCarro(identificador);
            
            var producto = await _context.productos.Include(p => p.Cat).FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        private void ProductosEnCarro(int identificador)
        {
            var productosEncarro = _context.Carro_productos.Include(p => p.Producto).Where(m => m.Carro.UsuarioId == identificador).Count();
            ViewBag.productosEnCarro = productosEncarro;
        }
        private bool ActualizarStockProducto(int idProducto, int cantidad)
        {
            Producto productoEncontrado = _context.productos.FirstOrDefault(producto => producto.ProductoId == idProducto);

            if (productoEncontrado != null)
            {
                productoEncontrado.Cantidad -= cantidad;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public IActionResult Comprar(int identificador, string Mensaje, string mensaje)
        {
            ViewBag.Mensaje = Mensaje;
            ViewBag.message = mensaje;

            ViewBag.identificador = identificador;

            Double precioTotal = 0;

            double IVA = 21;
            Usuario usuario = _context.usuarios.Where(usuario => usuario.UsuarioId == identificador).FirstOrDefault();
            Carro carrito = _context.carro.Include(c => c.Carro_productos).Include(p => p.ProductosCompra).FirstOrDefault(usuario => usuario.Usuario.UsuarioId == identificador);
            List<Carro_productos> sinStock = new List<Carro_productos>();
            List<Carro_productos> stockCantidad = new List<Carro_productos>();

            int sePudoComprar = 0;

            try
            {
                foreach (Carro_productos cp in carrito.Carro_productos)
                {
                    if (cp.Id_Carro == carrito.CarroId && cp.Id_Producto == cp.Producto.ProductoId)
                    {
                        if (cp.Producto.Cantidad <= 0)
                        {
                            sinStock.Add(cp);
                        }
                        else if (cp.Cantidad > cp.Producto.Cantidad)
                        {
                            stockCantidad.Add(cp);
                            cp.Cantidad = cp.Producto.Cantidad;
                            _context.productos.Update(cp.Producto);
                            _context.SaveChanges();
                        }
                        else
                        {
                            precioTotal += cp.Cantidad * cp.Producto.Precio;
                        }
                    }
                }

                if (sinStock.Count() != 0 && stockCantidad.Count() != 0)
                {
                    sePudoComprar = 5;
                }
                else if (sinStock.Count() != 0)
                {
                    sePudoComprar = 2;
                }
                else if (stockCantidad.Count() != 0)
                {
                    sePudoComprar = 3;
                }
                if (sePudoComprar == 5)
                {

                    foreach (Carro_productos cp in sinStock)
                    {
                        _context.Carro_productos.Remove(cp);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Mercado", new { Mensaje = "2", identificador = identificador });
                }

                if (sePudoComprar == 2)
                {

                    foreach (Carro_productos cp in sinStock)
                    {
                        _context.Carro_productos.Remove(cp);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Mercado", new { Mensaje = "3", identificador = identificador });
                }
                else if (sePudoComprar == 3)
                {
                    return RedirectToAction("Index", "Mercado", new { Mensaje = "4", identificador = identificador });
                }
                else
                {
                    precioTotal = MercadoHelper.CalcularPorcentaje(precioTotal, IVA);
                    Compra compra = new Compra(usuario.UsuarioId, precioTotal);
                    _context.compras.Add(compra);
                    _context.SaveChanges();
                    sePudoComprar = 1;

                    try
                    {
                        foreach (Carro_productos cp in carrito.Carro_productos)
                        {

                            compra.CompraProducto.Add(cp.Producto);
                            _context.compras.Update(compra);
                            _context.SaveChanges();
                            if (cp.Id_Carro == carrito.CarroId && cp.Id_Producto == cp.Producto.ProductoId)
                                compra.Productos_compra.Last<Productos_compra>().Cantidad_producto = cp.Cantidad;
                            ActualizarStockProducto(cp.Id_Producto, cp.Cantidad);
                            _context.compras.Update(compra);
                            _context.SaveChanges();
                        }

                        Vaciar(identificador);

                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Index", "Mercado", new { Mensaje = "5", identificador = identificador });
                    }

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Mercado", new { Mensaje = "5", identificador = identificador });
            }

            return RedirectToAction("Index", "Mercado", new { Mensaje = "1", identificador = identificador });

        }


        public async Task<IActionResult> Carro(int id, int identificador)
        {
            ViewBag.identificador = identificador;
            var producto = await _context.Carro_productos.Include(p => p.Producto).Where(m => m.Carro.UsuarioId == id).ToListAsync();
            ViewBag.CuentaProductos = producto.Count();
            ProductosEnCarro(identificador);
            return View(await _context.Carro_productos.Include(p => p.Producto).Where(m => m.Carro.UsuarioId == id).ToListAsync());
        }
        


        //[HttpPost]
        public IActionResult AgregarProducto(int ProductoId, int Cantidad, int identificador)
        {
            int usuarioID = identificador;
            if (ModelState.IsValid)
            {
                Producto productoEncontrado = _context.productos.Include(c => c.Carro_productos).FirstOrDefault(p => p.ProductoId == ProductoId);
                Carro carrito = _context.carro.Include(c => c.Carro_productos).Include(p => p.ProductosCompra).FirstOrDefault(usuario => usuario.Usuario.UsuarioId == identificador);
                Carro_productos carProd = _context.Carro_productos.Where(cp => cp.Id_Carro == carrito.CarroId).FirstOrDefault();

                if (carProd != null && carProd.Id_Carro == carrito.CarroId && carProd.Id_Producto == productoEncontrado.ProductoId)
                {
                    carProd.Cantidad += Cantidad;
                    _context.carro.Update(carrito);
                    _context.SaveChanges();
                }
                else
                {
                    carrito.ProductosCompra.Add(productoEncontrado);
                    _context.carro.Update(carrito);
                    _context.SaveChanges();
                    carrito.Carro_productos.Last<Carro_productos>().Cantidad = Cantidad;
                    _context.carro.Update(carrito);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Mercado", new { identificador = identificador });
        }

        public IActionResult QuitarDelCarro(int? id, int identificador)
        {
            Usuario usuario = _context.usuarios.Where(u => u.UsuarioId == identificador).Include(c => c.Carro).FirstOrDefault();
            Producto productoEncontrado = _context.productos.Where(p => p.ProductoId == id).FirstOrDefault();
            Carro carrito = usuario.Carro;
            Carro_productos prodABorrar = _context.Carro_productos.Where(carro => carro.Carro.UsuarioId == identificador && carro.Id_Producto == id).FirstOrDefault();
            _context.Carro_productos.Remove(prodABorrar);
            _context.SaveChanges();

            return RedirectToAction("Index", "Mercado", new { identificador = identificador });
        }

        public void Vaciar(int Id_Usuario)
        {
            var carroABorrar = _context.Carro_productos.Where(carro => carro.Carro.UsuarioId == Id_Usuario);
            _context.Carro_productos.RemoveRange(carroABorrar);
            _context.SaveChanges();
        }

        public IActionResult VaciarCarro(int identificador)
        {
            ViewBag.identificador = identificador;
            Vaciar(identificador);
            return RedirectToAction("Index", "Mercado", new { identificador = identificador });
        }

        public async Task<IActionResult> MisDatos(int? id, int identificador)
        {
            ViewBag.identificador = identificador;
            ProductosEnCarro(identificador);
            var usuario = await _context.usuarios.FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public async Task<IActionResult> MisCompras(int id, int identificador)
        {
            ViewBag.identificador = identificador;
            ProductosEnCarro(identificador);
            return View(await _context.compras.Where(u => u.idUsuario == id).OrderByDescending(u => u.CompraId).ToListAsync());
        }


        public async Task<IActionResult> DetailsCompras(int identificador, int id)
        {
            ViewBag.Identificador = identificador;
            ProductosEnCarro(identificador);
            return View(await _context.productos_compra.Include(p => p.Producto).Where(u => u.Id_compra == id).OrderBy(u => u.Id_compra).ToListAsync());
        }

        public async Task<IActionResult> EditData(int? id, string mensaje, int identificador)
        {
            ViewBag.Mensaje = mensaje;
            ProductosEnCarro(identificador);

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
        public IActionResult EditData(int identificador, string mensaje, int id, string nueva1, string nueva2, long Cuil, string Nombre, string Apellido, string Mail, string Password, int MiCarro, int TipoUsuario)
        {
            ViewBag.Identificador = identificador;
            ViewBag.Mensaje = mensaje;
            ProductosEnCarro(identificador);
            Password = RegistrarController.Encrypt.GetSHA256(Password);
            Usuario usu = _context.usuarios.FirstOrDefault(m => m.UsuarioId == id && m.Password == Password);

            if (usu == null)
            {
                ViewBag.Identificador = id;
                return RedirectToAction("EditData", "Mercado", new { mensaje = "Contraseña incorrecta", identificador = id });

            }

            if (ModelState.IsValid)
            {
                try
                {
                    usu.Cuil = Cuil;
                    usu.Nombre = Nombre;
                    usu.Apellido = Apellido;
                    usu.Mail = Mail;

                    if (nueva1 != null)
                    {
                        usu.Password = RegistrarController.Encrypt.GetSHA256(nueva2);
                    }

                    _context.Update(usu);
                    _context.SaveChanges();
                    ViewBag.identificador = usu.UsuarioId;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usu.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Mercado", new { identificador = usu.UsuarioId });
            }
            return View(usu);
        }

        public IActionResult EditarCarro(int? id, string mensaje, int identificador)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.identificador = identificador;
            ProductosEnCarro(identificador);

            if (id == null)
            {
                return NotFound();
            }

            var producto = _context.Carro_productos.Include(p => p.Producto).Where(cp => cp.Carro.UsuarioId == identificador && cp.Id_Producto ==id).FirstOrDefault();
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult EditProductoCarro(int identificador, int Cantidad, int Id_Producto, string mensaje)
        {
            Carro_productos cp = _context.Carro_productos.Include(c=> c.Carro).Include(p=> p.Producto).Where(cp => cp.Carro.UsuarioId == identificador && cp.Id_Producto == Id_Producto).FirstOrDefault();

            if(Cantidad > cp.Producto.Cantidad)
            {
                cp.Cantidad = cp.Producto.Cantidad;
                _context.Carro_productos.Update(cp);
            }
            else if(Cantidad == 0)
            {
                _context.Carro_productos.Remove(cp);
                _context.SaveChanges();
            }
            else
            {
                cp.Cantidad = Cantidad;
                _context.Carro_productos.Update(cp);
            }

            _context.SaveChanges();
            ViewBag.identificador = cp.Carro.UsuarioId;
            return RedirectToAction("Index", "Mercado", new { identificador = identificador, mensaje= "6" });
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
