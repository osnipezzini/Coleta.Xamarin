using AutoMapper;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;

namespace SOColeta.Domain.MapperProfiles;

public class StokProfile : Profile
{
    public StokProfile()
    {
        CreateMap<Product, ColetaModel>()
                .ForMember(dest => dest.ProdutoCode, map => map.MapFrom(src => src.Code))
                .ForMember(dest => dest.ProdutoCostPrice, map => map.MapFrom(src => src.CostPrice))
                .ForMember(dest => dest.ProdutoNome, map => map.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProdutoQuantity, map => map.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProdutoSalePrice, map => map.MapFrom(src => src.SalePrice))
                .ForMember(dest => dest.ProdutoId, map => map.MapFrom(src => src.Grid))
                .ForMember(dest => dest.ProdutoDeposito, map => map.MapFrom(src => src.Deposito))
                .ForMember(dest => dest.ProdutoGroupId, map => map.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Barcode, map => map.Ignore())
                .ReverseMap();

        CreateMap<Coleta, ColetaModel>()
            .ForMember(dest => dest.Quantidade, map => map.MapFrom(src => src.Quantidade))
            .ForMember(dest => dest.Barcode, map => map.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.HoraColeta, map => map.MapFrom(src => src.HoraColeta))
            .ForMember(dest => dest.ProdutoCode, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Code))
            .ForMember(dest => dest.ProdutoNome, map => map.MapFrom(src => src.Produto == null ? "" : src.Produto.Name))
            .ForMember(dest => dest.ProdutoQuantity, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Quantity))
            .ForMember(dest => dest.ProdutoSalePrice, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.SalePrice))
            .ForMember(dest => dest.ProdutoId, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Grid))
            .ForMember(dest => dest.ProdutoDeposito, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.Deposito))
            .ForMember(dest => dest.ProdutoGroupId, map => map.MapFrom(src => src.Produto == null ? 0 : src.Produto.GroupId))
            .ReverseMap();
    }
}
