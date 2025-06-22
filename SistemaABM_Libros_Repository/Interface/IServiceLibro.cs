using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServiceLibro
    {
        Task<IEnumerable<LibroDTO?>> GetAll();
        Task<LibroDTO?> GetById(int id);
        Task<ResponseApi> Create(LibroDTO categoria);
        Task<ResponseApi> Update(LibroDTO categoria);
        Task<ResponseApi> Delete(int id);
    }
}
