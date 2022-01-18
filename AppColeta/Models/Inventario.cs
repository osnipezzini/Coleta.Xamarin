using SQLite;

using System;

namespace SOColeta.Models
{
    public class Inventario
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
