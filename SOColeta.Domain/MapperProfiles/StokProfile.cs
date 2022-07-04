using AutoMapper;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Domain.MapperProfiles;

public class StokProfile : Profile
{
    public StokProfile()
    {
        CreateMap<Coleta, ColetaModel>()
            .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantidade))
            .ForMember(dest => dest.Barcode, map => map.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.HoraColeta, map => map.MapFrom(src => src.HoraColeta))
            .ForMember(dest => dest.Inventario, map => map.MapFrom(src => src.InventarioGuid))
            .ForMember(dest => dest.ProdutoId, map => map.MapFrom(src => src.ProdutoId == null ? 0 : src.ProdutoId))
            .ReverseMap();

        CreateMap<Product, ProdutoModel>()
            .ForMember(dest => dest.Grupo, map => map.MapFrom(src => src.GroupId))
            .ForMember(dest => dest.CodigoBarra, map => map.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.Codigo, map => map.MapFrom(src => src.Code))
            .ForMember(dest => dest.PrecoCusto, map => map.MapFrom(src => src.CostPrice))
            .ForMember(dest => dest.Nome, map => map.MapFrom(src => src.Name))
            .ForMember(dest => dest.PrecoUnit, map => map.MapFrom(src => src.SalePrice))
            .ForMember(dest => dest.Grid, map => map.MapFrom(src => src.Grid))
            .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantity))
            .ReverseMap();

    }
}
