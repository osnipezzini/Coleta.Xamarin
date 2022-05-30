using AutoMapper;

using SOColeta.Models;

namespace SOColeta.Profiles;

internal class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Common.Models.Inventario, Inventario>()
            .ReverseMap();
    }
}
