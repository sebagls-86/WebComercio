using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebComercio
{
    public class Usuario : IComparable<Usuario>
    {
        [Display(Name="Id de Usuario")]
        public int UsuarioId { get; set; }
        public int Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Carro Carro { get; set; }
        [Display(Name = "Id de Carro")]
        public int MiCarro { get; set; }
        [Display(Name = "Tipo de Usuario")]
        public int TipoUsuario { get; set; }
        public int Intentos { get; set; }
        public bool Bloqueado { get; set; }
        public List<Compra> Compra { get; set; }


        public Usuario(int cuil, string nombre, string apellido, string mail, string password, int MiCarro, int tipoUsuario)
        {
            Carro = new Carro();
            Cuil = cuil;
            Nombre = nombre;
            Apellido = apellido;
            Mail = mail;
            Password = password;
            Carro.CarroId = MiCarro;
            TipoUsuario = tipoUsuario;
        }

        public Usuario() {}

        public int CompareTo(Usuario other)
        {
            return Cuil.CompareTo(other.Cuil);
        }

        public override string ToString()
        {
            return $"{UsuarioId}|{Cuil}|{Nombre}|{Apellido}|{Mail}|{Password}|{TipoUsuario}";
        }

        public string[] toArray()
        {
            return new string[] { UsuarioId.ToString(), Cuil.ToString(), Nombre.ToString(), Apellido.ToString(), Mail.ToString(),
                MiCarro.ToString(),TipoUsuario.ToString() };
        }
    }
}
