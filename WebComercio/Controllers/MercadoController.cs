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



    }
}
