using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOColeta.Models
{
    public class Inventario
    {
        public string Id { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<Coleta> ProdutosColetados { get; set; } = new List<Coleta>();
        public bool IsFinished { get; set; } = false;
    }
}
