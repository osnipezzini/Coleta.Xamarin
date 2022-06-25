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
            .ForMember(dest => dest.ProdutoCode, map => map.MapFrom(src => src.Produto == null ? "" : src.Produto.Code))
            .ForMember(dest => dest.ProdutoNome, map => map.MapFrom(src => src.Produto == null ? "" : src.Produto.Name))
            .ForMember(dest => dest.ProdutoQuantity, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Quantity))
            .ForMember(dest => dest.ProdutoSalePrice, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.SalePrice))
            .ForMember(dest => dest.ProdutoId, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Grid))
            .ForMember(dest => dest.ProdutoDeposito, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Deposito))
            .ForMember(dest => dest.ProdutoGroupId, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.GroupId))
            .ReverseMap();
    }
}
