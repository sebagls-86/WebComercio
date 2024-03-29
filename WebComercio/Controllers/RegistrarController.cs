﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebComercio.Data;
using System.Security.Cryptography;
using System.Text;

namespace WebComercio.Controllers
{
    public class RegistrarController : Controller
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

        public RegistrarController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index(string mensaje, int identificador)
        {
            ViewBag.Mensaje = mensaje;

            ViewBag.Identificador = identificador;

            return View();
        }
        public IActionResult Registrado([Bind("UsuarioId,Cuil,Nombre,Apellido,Mail,Password,TipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Usuario usu = _context.usuarios.Where(u => u.Cuil == usuario.Cuil).FirstOrDefault();

                    if (usu != null)
                    {
                        TempData["Mensaje"] = "El CUIL " + usu.Cuil + " ya se encuentra registrado";
                        TempData["TipoMensaje"] = 1;
                        return RedirectToAction("Index", "Registrar", new { mensaje = "CUIL ya registrado", identificador = 1 });
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
                        return RedirectToAction("Index", "Login", new { Mensaje = "Se ha creado el usuario " + usuario.Cuil + " exitosamente." });
                    }
                }
                catch (Exception)
                {
                    TempData["Mensaje"] = "Error al crear usuario.";
                    TempData["TipoMensaje"] = 1;
                    return RedirectToAction("Index", "Registrar", new { mensaje = "Error al crear usuario.", identificador = 1 });
                }

            }
            return View(usuario);
        }

        public class Encrypt
        {
            public static string GetSHA256(string str)
            {
                SHA256 sha256 = SHA256Managed.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }

        }
    }
}


