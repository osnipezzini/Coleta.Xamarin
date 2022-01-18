using SQLite;

using System;

namespace SOColeta.Models
{
    public class Coleta
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public double Quantidade { get; set; }
        [Indexed]
        public int InventarioId { get; set; }
        public Inventario Inventario { get; set; }
        [Indexed]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public DateTime Hora { get; set; }
    }
}
