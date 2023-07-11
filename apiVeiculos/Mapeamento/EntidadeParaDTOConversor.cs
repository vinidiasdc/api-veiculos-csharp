using AutoMapper;
using apiVeiculos.DTOs;
using apiVeiculos.Entidades;

namespace apiVeiculos.Mapeamento
{
    public class EntidadeParaDTOConversor : Profile
    {
        public EntidadeParaDTOConversor()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Veiculo, VeiculoDTO>().ReverseMap();

            CreateMap<Veiculo, VeiculoCategoriaDTO>()
                .ForMember(dto => dto.NomeCategoria, opt => opt.MapFrom(src => src.Categoria.Id));
        }
    }
}
