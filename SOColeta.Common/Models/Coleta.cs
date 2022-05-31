using SOCore.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOColeta.Common.Models;

[Table("socoleta_coletas", Schema = "sotech")]
public class Coleta : ObservableObject
{
    #region Variáveis privadas
    private int id;
    private string codigo;
    private double quantidade;
    private int? produtoId;
    private Product product;
    private DateTime horaColeta;
    private int inventarioId;
    private Inventario inventario;
    #endregion
    #region Propriedades
    public int Id { get => id; set => SetProperty(ref id, value); }
    public string Codigo { get => codigo; set => SetProperty(ref codigo, value); }
    public double Quantidade { get => quantidade; set => SetProperty(ref quantidade, value); }
    public int? ProdutoId { get => produtoId; set => SetProperty(ref produtoId, value); }
    public Product Produto { get => product; set => SetProperty(ref product, value); }
    public DateTime HoraColeta { get => horaColeta; set => SetProperty(ref horaColeta, value); }
    public int InventarioId { get => inventarioId; set => SetProperty(ref inventarioId, value); }
    [Required]
    public Inventario? Inventario { get => inventario; set => SetProperty(ref inventario, value); }
    #endregion
    #region Construtores
    public Coleta()
    {

    }
    public Coleta(string barcode, double quantidade, DateTime horaColeta)
    {
        Quantidade = quantidade;
        Codigo = barcode;
        HoraColeta = horaColeta;
    }
    #endregion
}
