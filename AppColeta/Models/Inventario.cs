using System;
using System.Collections.Generic;

namespace AppColeta.Models
{
    public class Inventario
    {
        public string Id { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<Coleta> ProdutosColetados { get; set; }
    }
}
