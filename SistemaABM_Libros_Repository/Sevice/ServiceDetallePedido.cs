using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServiceDetallePedido : IServiceDetallePedido
    {
        private readonly IGenericRepository<DetallePedido> _repoDetalle;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceDetallePedido> _logger;

        public ServiceDetallePedido(IGenericRepository<DetallePedido> repoDetalle, IMapper mapper, ILogger<ServiceDetallePedido> logger)
        {
            _repoDetalle = repoDetalle;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(DetallePedidoDTO nuevoDetalle)
        {
            try
            {
                if (nuevoDetalle == null)
                {
                    _logger.LogWarning("Método Crear llamado con DetallePedidoDTO nulo.");
                    return new ResponseApi("Los datos del detalle del pedido no pueden ser nulos.", false);
                }

                var entidad = _mapper.Map<DetallePedido>(nuevoDetalle);
                await _repoDetalle.AddAsync(entidad);
                _logger.LogInformation("Detalle de pedido creado exitosamente para PedidoID: {IdPedido}", nuevoDetalle.Id);
                return new ResponseApi("El detalle del pedido fue creado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el detalle del pedido.");
                return new ResponseApi($"Error al crear el detalle del pedido. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var existente = await _repoDetalle.GetByIdAsync(id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de eliminar detalle de pedido inexistente con ID: {Id}", id);
                    return new ResponseApi("Detalle del pedido no encontrado.", false);
                }

                await _repoDetalle.DeleteAsync(id);
                _logger.LogInformation("Detalle de pedido con ID: {Id} eliminado exitosamente.", id);
                return new ResponseApi("Detalle del pedido eliminado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el detalle del pedido con ID: {Id}", id);
                return new ResponseApi($"Error al eliminar el detalle del pedido. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<DetallePedidoDTO?>> GetAll()
        {
            try
            {
                var detalles = await _repoDetalle.GetAllAsync();
                return _mapper.Map<IEnumerable<DetallePedidoDTO>>(detalles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los detalles de pedido.");
                return Enumerable.Empty<DetallePedidoDTO>();
            }
        }

        public async Task<DetallePedidoDTO?> GetById(int id)
        {
            try
            {
                var detalle = await _repoDetalle.GetByIdAsync(id);
                return _mapper.Map<DetallePedidoDTO>(detalle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar detalle de pedido con ID: {Id}", id);
                return null;
            }
        }

        public async Task<IEnumerable<DetallePedidoDTO>> GetByPedidoId(int pedidoId)
        {
            try
            {
                var detalles = await _repoDetalle.GetByIdAsync(pedidoId);
                return _mapper.Map<IEnumerable<DetallePedidoDTO>>(detalles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles para pedido {pedidoId}");
                return Enumerable.Empty<DetallePedidoDTO>();
            }
        }


        public async Task<ResponseApi> Update(DetallePedidoDTO detalleActualizar)
        {
            try
            {
                if (detalleActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con DetallePedidoDTO nulo.");
                    return new ResponseApi("Los datos del detalle del pedido no pueden ser nulos.", false);
                }

                var existente = await _repoDetalle.GetByIdAsync(detalleActualizar.Id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de actualizar detalle de pedido inexistente con ID: {Id}", detalleActualizar.Id);
                    return new ResponseApi("Detalle del pedido no encontrado.", false);
                }

                _mapper.Map(detalleActualizar, existente);
                await _repoDetalle.UpdateAsync(existente);
                _logger.LogInformation("Detalle de pedido actualizado exitosamente con ID: {Id}", detalleActualizar.Id);
                return new ResponseApi("Detalle del pedido actualizado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el detalle del pedido con ID: {Id}", detalleActualizar?.Id);
                return new ResponseApi($"Error al actualizar el detalle del pedido. Detalles: {ex.Message}", false);
            }
        }
    }
}
