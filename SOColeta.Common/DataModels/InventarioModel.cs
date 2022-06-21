using SOCore.Models;

using System.Collections.ObjectModel;

namespace SOColeta.Common.DataModels;

public class InventarioModel : ObservableObject
{
    #region Váriaveis privadas
    private int? id = 0;
    private Guid? guid;
    private long? empresa = 0;
    private DateTime dataCriacao;
    private string? nomeDoArquivo = "";
    private bool isValid = false;
    private bool isInserted = false;
    private ObservableCollection<ColetaModel> coletas;
    #endregion
    public InventarioModel()
    {
    }
    public InventarioModel(Guid? guid, DateTime dataCriacao)
    {
        Guid = guid;
        DataCriacao = dataCriacao;
    }

    public int? Id { get => id; set => SetProperty(ref id, value); }
    public Guid? Guid { get => guid; set => SetProperty(ref guid, value); }
    public string? NomeArquivo { get => nomeDoArquivo; set => SetProperty(ref nomeDoArquivo, value); }
    public DateTime DataCriacao { get => dataCriacao; set => SetProperty(ref dataCriacao, value); }
    public bool IsValid { get => isValid; set => SetProperty(ref isValid, value); }
    public bool IsInserted { get => isInserted; set => SetProperty(ref isInserted, value); }
    public string Device { get; set; }
    public long? Empresa { get => empresa; set => SetProperty(ref empresa, value); }
    public ObservableCollection<ColetaModel> Coletas
    {
        get => coletas ??= new ObservableCollection<ColetaModel>();
    }
}
