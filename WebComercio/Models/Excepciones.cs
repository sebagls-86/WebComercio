﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebComercio
{
   public class Excepciones: Exception
    {
        public Excepciones(String mensaje) : base(mensaje)
        {


        }

    }
}
