using SOColeta.Common.DataModels;

using SOCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOColeta.Common.Models;

[Table("socoleta_products", Schema = "sotech")]
public class Product : ObservableObject
{
    #region Variáveis privadas
    private int id;
    private Guid? guid;
    private double quantity;
    private long deposito;
    private long grid;
    private string? code;
    private string name;
    private double? salePrice;
    private double? costPrice;
    private long groupId;
    private string barcode;
    #endregion
    #region Construtores
    public Product()
    {

    }
    public Product(ColetaModel model)
    {
        Quantity = model.ProdutoQuantity;
        Deposito = model.ProdutoDeposito;
        Grid = model.ProdutoId;
        Barcode = model.Barcode;
        CostPrice = model.ProdutoCostPrice;
        SalePrice = model.ProdutoSalePrice;
        GroupId = model.ProdutoGroupId;
        Name = model.ProdutoNome;
        Code = model.ProdutoCode;
    }
    #endregion
    #region Propriedades
    public int Id { get => id; set => SetProperty(ref id, value); }
    public Guid? Guid { get => guid; set => SetProperty(ref guid, value); }
    public double Quantity { get => quantity; set => SetProperty(ref quantity, value); }
    public long Deposito { get => deposito; set => SetProperty(ref deposito, value); }
    public long Grid { get => grid; set => SetProperty(ref grid, value); }
    public string? Code { get => code; set => SetProperty(ref code, value); }
    public string Name { get => name; set => SetProperty(ref name, value); }
    public double? CostPrice { get => costPrice; set => SetProperty(ref costPrice, value); }
    public double? SalePrice { get => salePrice; set => SetProperty(ref salePrice, value); }
    public long GroupId { get => groupId; set => SetProperty(ref groupId, value); }
    public string Barcode { get => barcode; set => SetProperty(ref barcode, value); }
    #endregion
}
