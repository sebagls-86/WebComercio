using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(Producto producto)
        {
            List<Producto> productosOrdenados = new List<Producto>();
            productosOrdenados = _context.productos.OrderBy(producto => producto.ProductoId).ToList();

            ViewBag.Productos = productosOrdenados;

            //ViewBag.Identificador = identificador;
            return View();
        }
    }
}
