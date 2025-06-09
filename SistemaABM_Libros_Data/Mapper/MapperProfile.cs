using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Data.Mapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile() {

            CreateMap<Usuario, UsuarioDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UsuarioId))
                 .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashContraseña(src.Contraseña)))
                .ForMember(dest => dest.Pedidos, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento));


        }

        private string HashContraseña(string contraseña)
        {
            return $"HASHEADA_{contraseña}_SEGURAMENTE";
        }
    }
}
