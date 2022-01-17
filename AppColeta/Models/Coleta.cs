namespace SOColeta.Models
{
    public class Coleta
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public double Quantidade { get; set; }
        public string InventarioId { get; set; }
        public Inventario Inventario { get; set; }
    }
}
