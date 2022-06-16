using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Common.Services;

public interface IColetaService
{
    Task<Coleta?> AddColeta(ColetaModel coletaModel);
    Task<ColetaModel?> GetColeta(string codigo, Guid? inventarioGuid);
}
