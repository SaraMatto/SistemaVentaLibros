using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServiceCategoria
    {
        Task<IEnumerable<CategoriaDTO?>> GetAll();
        Task<CategoriaDTO?> GetById(int id);
        Task<ResponseApi> Create(CategoriaDTO categoria);
        Task<ResponseApi> Update(CategoriaDTO categoria);
        Task<ResponseApi> Delete(int id);
    }
}
