using SOColeta.Common.Models;

using SOCore.Models;

namespace SOColeta.Common.DataModels;

public class ColetaModel : ObservableObject
{
    #region Váriaveis privadas
    private string barcode;
    private Guid? inventario;
    private long? produtoId;
    private double quantity;
    private DateTime horaColeta;
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
        ProdutoId = coleta.ProdutoId;
        HoraColeta = coleta.HoraColeta;
    }
    #endregion
    #region Propriedades
    public string Barcode { get => barcode; set => SetProperty(ref barcode, value); }
    public Guid? Inventario { get => inventario; set => SetProperty(ref inventario, value); }
    public long? ProdutoId { get => produtoId; set => SetProperty(ref produtoId, value); }
    public double Quantidade { get => quantity; set => SetProperty(ref quantity, value); }
    public DateTime HoraColeta { get => horaColeta; set => SetProperty(ref horaColeta, value); }
    #endregion
}
