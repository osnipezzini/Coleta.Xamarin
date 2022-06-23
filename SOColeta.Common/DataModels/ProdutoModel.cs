namespace SOColeta.Common.DataModels;

public class ProdutoModel
{
    public int Codigo { get; set; }
    public long? Grid { get; set; }
    public string Nome { get; set; }
    public double? Quantidade { get; set; }
    public double PrecoCusto { get; set; }
    public double PrecoUnit { get; set; }
    public int Grupo { get; set; }
    public string CodigoBarra {get; set; }
}
