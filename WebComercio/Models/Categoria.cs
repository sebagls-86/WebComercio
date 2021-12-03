using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebComercio
{
    public class Categoria : IComparable<Categoria>
    {   
        [Key]
        public int CatId { get; set; }
        public string Nombre { get; set; }

        public List<Producto> Productos { get; set; } = new List<Producto>();

        public Categoria() {}
        public Categoria(string nombre)
        {
            
            Nombre = nombre;
        }


        public int CompareTo(Categoria other)
        {
            return Nombre.CompareTo(other.Nombre);
        }

        public override string ToString()
        {
            return $"{CatId}|{Nombre}";
        }

        public string[] toArray()
        {
            return new string[] { CatId.ToString(), Nombre.ToString() };
        }
    }
}
