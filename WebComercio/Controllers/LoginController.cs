using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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


        public IActionResult Login(int Cuil, string Password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Usuario usuario = _context.usuarios.Where(u => u.Cuil == Cuil).FirstOrDefault();

                    if (usuario != null)
                    {
                        if (!usuario.Bloqueado && usuario.Intentos < 3)
                        {
                            Password = RegistrarController.Encrypt.GetSHA256(Password);
                            if (usuario.Password == Password)
                            {
                                usuario.Intentos = 0;
                                _context.usuarios.Update(usuario);
                                _context.SaveChanges();

                                if (usuario.TipoUsuario == 1)
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Mercado", new { identificador = usuario.Cuil });
                                }
                            }
                            else
                            {
                                usuario.Intentos++;
                                _context.usuarios.Update(usuario);
                                _context.SaveChanges();
                                return RedirectToAction("Index", "Login", new { mensaje = "Usuario o contraseña incorrectos", identificador = 0 });
                            }
                        }
                        else if (!usuario.Bloqueado)
                        {
                            usuario.Bloqueado = true;
                            usuario.Intentos = 0;
                            _context.usuarios.Update(usuario);
                            _context.SaveChanges();
                            return RedirectToAction("Index", "Login", new { mensaje = "Usuario bloqueado", identificador = 0 });
                        }
                        else if (usuario.Bloqueado)
                        {
                            return RedirectToAction("Index", "Login", new { mensaje = "Usuario bloqueado. Comuniquese con el administrador", identificador = 0 });
                        }
                    } else
                    {
                            return RedirectToAction("Index", "Login", new { mensaje = "El usuario no existe", identificador = 0 });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Login", new { mensaje = "Error", identificador = -1 });
                }

            }
            return View(Cuil);
        }
    }
}


