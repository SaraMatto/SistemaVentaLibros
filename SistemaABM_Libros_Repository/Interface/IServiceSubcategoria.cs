using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServiceSubcategoria
    {
        Task<IEnumerable<SubcategoriaDTO?>> GetAll();
        Task<SubcategoriaDTO?> GetById(int id);
        Task<ResponseApi> Create(SubcategoriaDTO categoria);
        Task<ResponseApi> Update(SubcategoriaDTO categoria);
        Task<ResponseApi> Delete(int id);
    }
}
