using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServicePedido
    {
        Task<IEnumerable<PedidoDTO?>> GetAll();
        Task<PedidoDTO?> GetById(int id);
        Task<ResponseApi> Create(PedidoDTO categoria);
        Task<ResponseApi> Update(PedidoDTO categoria);
        Task<ResponseApi> Delete(int id);

        Task<IEnumerable<PedidoDTO>> GetByUsuarioId(int usuarioId);
    }
}
