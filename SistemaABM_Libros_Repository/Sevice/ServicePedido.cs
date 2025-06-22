using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServicePedido : IServicePedido
    {
        private readonly IGenericRepository<Pedido> _repoPedido;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicePedido> _logger;

        public ServicePedido(IGenericRepository<Pedido> repoPedido, IMapper mapper, ILogger<ServicePedido> logger)
        {
            _repoPedido = repoPedido;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(PedidoDTO nuevoPedido)
        {
            try
            {
                if (nuevoPedido == null)
                {
                    _logger.LogWarning("Método Crear llamado con PedidoDTO nulo.");
                    return new ResponseApi("Los datos del pedido no pueden ser nulos.", false);
                }

                var entidad = _mapper.Map<Pedido>(nuevoPedido);
                await _repoPedido.AddAsync(entidad);
                _logger.LogInformation("Pedido creado exitosamente con ID Usuario: {IdUsuario}", nuevoPedido.UsuarioId);
                return new ResponseApi("El pedido fue creado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el pedido.");
                return new ResponseApi($"Error al crear el pedido. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var existente = await _repoPedido.GetByIdAsync(id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de eliminar pedido inexistente con ID: {Id}", id);
                    return new ResponseApi("Pedido no encontrado.", false);
                }

                await _repoPedido.DeleteAsync(id);
                _logger.LogInformation("Pedido con ID: {Id} eliminado exitosamente.", id);
                return new ResponseApi("Pedido eliminado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pedido con ID: {Id}", id);
                return new ResponseApi($"Error al eliminar el pedido. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<PedidoDTO?>> GetAll()
        {
            try
            {
                var pedidos = await _repoPedido.GetAllAsync();
                return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los pedidos.");
                return Enumerable.Empty<PedidoDTO>();
            }
        }

        public async Task<PedidoDTO?> GetById(int id)
        {
            try
            {
                var pedido = await _repoPedido.GetByIdAsync(id);
                return _mapper.Map<PedidoDTO>(pedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar pedido con ID: {Id}", id);
                return null;
            }
        }

        public async Task<ResponseApi> Update(PedidoDTO pedidoActualizar)
        {
            try
            {
                if (pedidoActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con PedidoDTO nulo.");
                    return new ResponseApi("Los datos del pedido no pueden ser nulos.", false);
                }

                var existente = await _repoPedido.GetByIdAsync(pedidoActualizar.Id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de actualizar pedido inexistente con ID: {Id}", pedidoActualizar.Id);
                    return new ResponseApi("Pedido no encontrado.", false);
                }

                _mapper.Map(pedidoActualizar, existente);
                await _repoPedido.UpdateAsync(existente);
                _logger.LogInformation("Pedido actualizado exitosamente con ID: {Id}", pedidoActualizar.Id);
                return new ResponseApi("Pedido actualizado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pedido con ID: {Id}", pedidoActualizar?.Id);
                return new ResponseApi($"Error al actualizar el pedido. Detalles: {ex.Message}", false);
            }
        }
    }
}
