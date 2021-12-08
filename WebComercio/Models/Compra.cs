using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebComercio
{
    public class Compra : IComparable<Compra>
    {
        [Display(Name = "Id de Compra")]
        public int CompraId { get; set; }
        
        public Usuario Usuario { get; set; }

        [Display(Name = "Id de Usuario")]
        public int idUsuario { get; set; }
        
        public Double Total { get; set; }
        public List<Productos_compra> Productos_compra { get; set; }
        public ICollection<Producto> CompraProducto { get; set; } = new List<Producto>();


        public Compra() { }

        public Compra(int _Id, Usuario usuario, Double total)
        {
            CompraId = _Id;
            Usuario = usuario;
            Total = total;
        }

        public Compra (int usuarioId, double total)
        {
            this.idUsuario = usuarioId;
            this.Total = total;
        }

        public int CompareTo(Compra other)
        {
            return CompraId.CompareTo(other.CompraId);
        }

        

        public override string ToString()
        {
            return $"{CompraId}|{Usuario.UsuarioId}|{Total}";
        }

        public string[] toArray()
        {
            return new string[] { CompraId.ToString(), Usuario.UsuarioId.ToString(), Total.ToString() };
        }
    }
}
