using System;
using System.Collections.Generic;


namespace WebComercio
{
    public class Carro
    {
        public int CarroId { get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
        public ICollection<Producto> ProductosCompra { get; set; } = new List<Producto>();
        public List<Carro_productos> Carro_productos { get; set; }
        
        public Carro() { }

    }
}
