using Microsoft.AspNetCore.Mvc;
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

        public async Task <IActionResult> Index(string mensaje, int identificador, string searchString, string Cat, string OrderByNombre, string OrderByPrecio, string sortOrder)
        {
            List<Producto> productosOrdenados = new List<Producto>();
            productosOrdenados = _context.productos.OrderBy(producto => producto.ProductoId).ToList();
            List<Categoria> categorias = new List<Categoria>();
            categorias = _context.categorias.ToList();

            ViewBag.Productos = productosOrdenados;
            ViewBag.categorias = categorias;

            ViewBag.Mensaje = mensaje;

            //ViewBag.Identificador = identificador;
            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewData["CurrentFilter"] = searchString;

            ////var productos = from p in _context.productos.Include(p => p.Cat)
            ////                select p;

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        productos = productos.OrderByDescending(p => p.Nombre);
            //        break;
            //    case "Date":
            //        productos = productos.OrderBy(p => p.Precio);
            //        break;
            //    case "date_desc":
            //        productos = productos.OrderByDescending(p => p.Precio);
            //        break;
            //    default:
            //        productos = productos.OrderBy(p => p.Nombre);
            //        break;
            //}



            var productos = from p in _context.productos.Include(p => p.Cat)
                            select p;


            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(u => u.Nombre.Contains(searchString));
                ViewBag.Productos = productos;
            }
            else if (!String.IsNullOrEmpty(Cat))
            {
                productos = productos.Where(u => u.Cat.Nombre.Contains(Cat));
                ViewBag.Productos = productos;

            }
            else if (OrderByNombre == "1")
            {
                productos = productos.OrderBy(u => u.Nombre);
                ViewBag.Productos = productos;
            }
            else if (OrderByNombre == "2")
            {
                ViewBag.Productos = productos.OrderByDescending(u => u.Nombre); ;
            }
            else if (OrderByPrecio == "1")
            {
                productos = productos.OrderByDescending(u => u.Precio);
                ViewBag.Productos = productos;
            }
            else if (OrderByPrecio == "2")
            {
                productos = productos.OrderBy(u => u.Precio);
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

    }
}
