using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebComercio.Data;

namespace WebComercio.Controllers
{
    public class SessionController : Controller
    {
        private readonly MyContext _context;

        public SessionController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string name = HttpContext.Session.GetString("Name");
            return View(model: name);
        }
        public IActionResult SaveName(string name)
        {
            HttpContext.Session.SetString("Name", name);
            return RedirectToAction("Index");
        }
    }
}
