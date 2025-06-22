using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros_Repository.Interface
{
    public interface IServiceDetallePedido
    {
        Task<IEnumerable<DetallePedidoDTO?>> GetAll();
        Task<DetallePedidoDTO?> GetById(int id);
        Task<ResponseApi> Create(DetallePedidoDTO categoria);
        Task<ResponseApi> Update(DetallePedidoDTO categoria);
        Task<ResponseApi> Delete(int id);
        Task<IEnumerable<DetallePedidoDTO>> GetByPedidoId(int pedidoId);
    }
}
