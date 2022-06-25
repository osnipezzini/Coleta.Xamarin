using SOColeta.Common.Models;

using SOCore.Models;

namespace SOColeta.Common.DataModels;

public class ColetaModel : ObservableObject
{
    #region Váriaveis privadas
    private string barcode;
    private Guid? inventario;
    private long produtoId;
    private string produtoNome;
    private double quantity;
    private DateTime horaColeta;
    private double produtoQuantity;
    private long produtoGroupId;
    private double? produtoSalePrice;
    private double? produtoCostPrice;
    private long produtoDeposito;
    private string produtoCode;
    #endregion
    #region Construtores
    public ColetaModel()
    {
    }

    public ColetaModel(Coleta coleta)
    {
        Quantidade = coleta.Quantidade;
        Barcode = coleta.Codigo;
        Inventario = coleta.InventarioGuid;
        ProdutoCode = coleta.Produto?.Code ?? "0";
        ProdutoCostPrice = coleta.Produto?.CostPrice;
        ProdutoSalePrice = coleta.Produto?.SalePrice;
        ProdutoDeposito = coleta.Produto?.Deposito ?? 0;
        ProdutoGroupId = coleta.Produto?.GroupId ?? 0;
        ProdutoId = coleta.Produto?.Grid ?? 0;
        HoraColeta = coleta.HoraColeta;
        ProdutoNome = coleta.Produto?.Name;
        ProdutoQuantity = coleta.Produto?.Quantity ?? 0;
    }
    #endregion
    #region Propriedades
    public string Barcode { get => barcode; set => SetProperty(ref barcode, value); }
    public Guid? Inventario { get => inventario; set => SetProperty(ref inventario, value); }
    public long ProdutoId { get => produtoId; set => SetProperty(ref produtoId, value); }
    public string ProdutoNome { get => produtoNome; set => SetProperty(ref produtoNome, value); }
    public double Quantidade { get => quantity; set => SetProperty(ref quantity, value); }
    public DateTime HoraColeta { get => horaColeta; set => SetProperty(ref horaColeta, value); }
    public double ProdutoQuantity { get => produtoQuantity; set => SetProperty(ref produtoQuantity, value); }
    public long ProdutoGroupId { get => produtoGroupId; set => SetProperty(ref produtoGroupId, value); }
    public double? ProdutoSalePrice { get => produtoSalePrice; set => SetProperty(ref produtoSalePrice, value); }
    public double? ProdutoCostPrice { get => produtoCostPrice; set => SetProperty(ref produtoCostPrice, value); }
    public long ProdutoDeposito { get => produtoDeposito; set => SetProperty(ref produtoDeposito, value); }
    public string ProdutoCode { get => produtoCode; set => SetProperty(ref produtoCode, value); }
    #endregion
}
