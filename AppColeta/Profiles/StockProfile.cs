using AutoMapper;
using SOColeta.Common.Models;

namespace SOColeta.Profiles;

internal class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Inventario, Inventario>()
            .ReverseMap();
    }
}
