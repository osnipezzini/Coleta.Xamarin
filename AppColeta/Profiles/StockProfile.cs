using AutoMapper;
using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Profiles;

internal class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Inventario, Inventario>()
            .ReverseMap();

        CreateMap<Coleta, ColetaModel>()
            .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantidade))
            .ForMember(dest => dest.Barcode, map => map.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.HoraColeta, map => map.MapFrom(src => src.HoraColeta))
            .ForMember(dest => dest.Inventario, map => map.MapFrom(src => src.InventarioGuid))
            .ForMember(dest => dest.ProdutoId, map => map.MapFrom(src => src.ProdutoId == null ? 0 : src.ProdutoId))
            .ReverseMap();
    }
}
