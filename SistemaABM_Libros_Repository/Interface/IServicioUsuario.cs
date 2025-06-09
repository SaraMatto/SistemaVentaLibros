using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServicioUsuario
    {
        Task<IEnumerable<UsuarioDTO?>> ObtenerTodosUsuariosAsync();
        Task<UsuarioDTO?> ObtenerUsuarioPorIdAsync(int id);
        Task<ResponseApi> CrearUsuarioAsync(UsuarioDTO nuevoUsuario);
        Task<ResponseApi> ActualizarUsuarioAsync(UsuarioDTO usuarioActualizar);
        Task<ResponseApi> EliminarUsuarioAsync(int id);
    }
}
