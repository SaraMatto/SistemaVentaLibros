using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_TranferObject.ModelsDTO;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Libro ↔ LibroDTO
        CreateMap<Libro, LibroDTO>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.LibroId))
            .ForMember(d => d.SubcategoriaId, o => o.MapFrom(s => s.SubcategoriaId))
            .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Titulo))
            // ... otras propiedades ...
            .ForMember(d => d.Imagen, o => o.MapFrom(s => s.Imagen))
            .ForMember(d => d.Subcategoria, o => o.MapFrom(s => s.Subcategoria));

        CreateMap<LibroDTO, Libro>()
            .ForMember(d => d.LibroId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.SubcategoriaId, o => o.MapFrom(s => s.SubcategoriaId))
            .ForMember(d => d.Imagen, o => o.MapFrom(s => s.Imagen))
            .ForMember(d => d.DetallePedidos, o => o.Ignore())
            .ForMember(d => d.Subcategoria, o => o.Ignore());

        // Subcategoria ↔ SubcategoriaDTO
        CreateMap<Subcategoria, SubcategoriaDTO>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.SubcategoriaId))
            .ForMember(d => d.CategoriaId, o => o.MapFrom(s => s.CategoriaId))
            .ForMember(d => d.NombreSubcategoria, o => o.MapFrom(s => s.NombreSubcategoria))
            .ForMember(d => d.Descripcion, o => o.MapFrom(s => s.Descripcion))
            .ForMember(d => d.Categoria, o => o.MapFrom(s => s.Categoria));

        CreateMap<SubcategoriaDTO, Subcategoria>()
            .ForMember(d => d.SubcategoriaId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Categoria, o => o.Ignore());

        // Categoria ↔ CategoriaDTO
        CreateMap<Categoria, CategoriaDTO>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.CategoriaId))
            .ForMember(d => d.NombreCategoria, o => o.MapFrom(s => s.NombreCategoria))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Descripcion));

        CreateMap<CategoriaDTO, Categoria>()
            .ForMember(d => d.CategoriaId, o => o.MapFrom(s => s.Id));

        // DetallePedido ↔ DetallePedidoDTO
        CreateMap<DetallePedido, DetallePedidoDTO>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.DetallePedidoId))
            .ForMember(d => d.TituloLibro, o => o.MapFrom(s => s.Libro.Titulo))
            .ForMember(d => d.Libro, o => o.MapFrom(s => s.Libro));

        CreateMap<DetallePedidoDTO, DetallePedido>()
            .ForMember(d => d.DetallePedidoId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Pedido, o => o.Ignore())
            .ForMember(d => d.Libro, o => o.Ignore());

        // Pedido ↔ PedidoDTO
        CreateMap<Pedido, PedidoDTO>()
            .ForMember(d => d.PedidoID, o => o.MapFrom(s => s.PedidoId))
            .ForMember(d => d.UsuarioId, o => o.MapFrom(s => s.UsuarioId))
            .ForMember(d => d.Detalles, o => o.MapFrom(s => s.DetallePedidos));

        CreateMap<PedidoDTO, Pedido>()
            .ForMember(d => d.PedidoId, o => o.MapFrom(s => s.PedidoID))
            .ForMember(d => d.Usuario, o => o.Ignore())
            .ForMember(d => d.DetallePedidos, o => o.Ignore());
    }
}
