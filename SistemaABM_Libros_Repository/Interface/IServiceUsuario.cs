using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServiceUsuario
    {
        Task<IEnumerable<UsuarioDTO?>> GetAll();
        Task<UsuarioDTO?> GetById(int id);
        Task<ResponseApi> Create(UsuarioDTO nuevoUsuario);
        Task<ResponseApi> Update(UsuarioDTO usuarioActualizar);
        Task<ResponseApi> Delete(int id);
    }
}
