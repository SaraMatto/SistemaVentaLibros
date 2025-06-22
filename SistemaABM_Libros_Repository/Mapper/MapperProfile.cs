using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Data.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashContraseña(src.Contraseña)))
                .ForMember(dest => dest.Pedidos, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento));

            // Categoria
            CreateMap<Categoria, CategoriaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoriaId));

            CreateMap<CategoriaDTO, Categoria>()
                .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.Id));

            // SubCategoria
            CreateMap<Subcategoria, SubcategoriaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubcategoriaId))
                .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.CategoriaId));

            CreateMap<SubcategoriaDTO, Subcategoria>()
                .ForMember(dest => dest.SubcategoriaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

            // Libro
            CreateMap<Libro, LibroDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LibroId))
                .ForMember(dest => dest.SubcategoriaId, opt => opt.MapFrom(src => src.SubcategoriaId))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Autor))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Isbn))
                .ForMember(dest => dest.TipoLibro, opt => opt.MapFrom(src => src.TipoLibro))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Idioma, opt => opt.MapFrom(src => src.Idioma))
                .ForMember(dest => dest.Editorial, opt => opt.MapFrom(src => src.Editorial))
                .ForMember(dest => dest.AnioPublicacion, opt => opt.MapFrom(src => src.AnioPublicacion))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.EstadoLibro, opt => opt.MapFrom(src => src.EstadoLibro));

            CreateMap<LibroDTO, Libro>()
                .ForMember(dest => dest.LibroId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubcategoriaId, opt => opt.MapFrom(src => src.SubcategoriaId))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Autor))
                .ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.TipoLibro, opt => opt.MapFrom(src => src.TipoLibro))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Idioma, opt => opt.MapFrom(src => src.Idioma))
                .ForMember(dest => dest.Editorial, opt => opt.MapFrom(src => src.Editorial))
                .ForMember(dest => dest.AnioPublicacion, opt => opt.MapFrom(src => src.AnioPublicacion))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.EstadoLibro, opt => opt.MapFrom(src => src.EstadoLibro))
                .ForMember(dest => dest.DetallePedidos, opt => opt.Ignore())
                .ForMember(dest => dest.Subcategoria, opt => opt.Ignore());

            // DetallePedido
            CreateMap<DetallePedido, DetallePedidoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DetallePedidoId));

            CreateMap<DetallePedidoDTO, DetallePedido>()
                .ForMember(dest => dest.DetallePedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Pedido, opt => opt.Ignore())
                .ForMember(dest => dest.Libro, opt => opt.Ignore());

            // Pedido
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PedidoId))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));

            CreateMap<PedidoDTO, Pedido>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.DetallePedidos, opt => opt.Ignore());
        }

        private string HashContraseña(string contraseña)
        {
            return $"HASHEADA_{contraseña}_SEGURAMENTE";
        }
    }
}
