using SOCore.Models;

using System.Collections.ObjectModel;

namespace SOColeta.Common.DataModels;

public class InventarioModel : ObservableObject
{
    #region Váriaveis privadas
    private int? id;
    private Guid? guid;
    private DateTime data;
    private ObservableCollection<ColetaModel> coletas;
    #endregion
    public InventarioModel()
    {
    }
    public InventarioModel(Guid? guid, DateTime dataCriacao)
    {
        Guid = guid;
        Data = dataCriacao;
    }

    public int? Id { get => id; set => SetProperty(ref id, value); }
    public Guid? Guid { get => guid; set => SetProperty(ref guid, value); }
    public DateTime Data { get => data; set => SetProperty(ref data, value); }
    public ObservableCollection<ColetaModel> Coletas
    {
        get => coletas ??= new ObservableCollection<ColetaModel>();
    }
}
