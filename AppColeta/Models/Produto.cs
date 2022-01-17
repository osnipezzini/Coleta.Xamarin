using System;
using System.Collections.Generic;
using System.Text;

namespace SOColeta.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public double PrecoVenda { get; set; }
        public double PrecoCusto { get; set; }
    }
}
