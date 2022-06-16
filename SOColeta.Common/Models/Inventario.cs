using SOCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOColeta.Common.Models;

[Table("socoleta_inventarios", Schema = "sotech")]
public class Inventario : ObservableObject
{
    #region Variáveis privadas
    private int? id;
    private Guid? guid;
    private long empresa;
    private string? nomeDoArquivo;
    private DateTime dataCriacao = DateTime.UtcNow;
    private bool isValid = false;
    private bool isInserted = false;
    private bool isFinished = false;
    #endregion
    public int? Id { get => id; set => SetProperty(ref id, value); }
    public Guid? Guid { get => guid; set => SetProperty(ref guid, value); }
    public string? NomeArquivo { get => nomeDoArquivo; set => SetProperty(ref nomeDoArquivo, value); }
    public DateTime DataCriacao { get => dataCriacao; set => SetProperty(ref dataCriacao, value); }
    public bool IsValid { get => isValid; set => SetProperty(ref isValid, value); }
    public bool IsInserted { get => isInserted; set => SetProperty(ref isInserted, value); }
    public bool IsFinished { get => isFinished; set => SetProperty(ref isFinished, value); }
    public List<Coleta>? ProdutosColetados { get; set; }
    public string Device { get; set; }
    public long Empresa { get => empresa; set => SetProperty(ref empresa, value); }
}
