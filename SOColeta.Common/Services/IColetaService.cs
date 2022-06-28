using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Common.Services;

public interface IColetaService
{
    Task AddColeta(ColetaModel coletaModel);
    Task<Coleta?> GetColeta(Guid? inventarioGuid);
}
