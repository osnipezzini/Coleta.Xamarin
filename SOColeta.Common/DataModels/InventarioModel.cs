using SOCore.Models;

using System.Collections.ObjectModel;

namespace SOColeta.Common.DataModels;

public class InventarioModel : ObservableObject
{
    #region Váriaveis privadas
    private int id;
    private DateTime data;
    private ObservableCollection<ColetaModel> coletas;
    #endregion
    public InventarioModel()
    {
    }
    public InventarioModel(int id, DateTime dataCriacao)
    {
        Id = id;
        Data = dataCriacao;
    }

    public int Id { get => id; set => SetProperty(ref id, value); }
    public DateTime Data { get => data; set => SetProperty(ref data, value); }
    public ObservableCollection<ColetaModel> Coletas
    {
        get => coletas ??= new ObservableCollection<ColetaModel>();
    }
}
