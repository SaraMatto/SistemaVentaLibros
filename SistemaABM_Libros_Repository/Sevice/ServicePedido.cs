using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServicePedido : IServicePedido
    {
        private readonly IGenericRepository<Pedido> _repoPedido;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicePedido> _logger;
        private readonly IGenericRepository<DetallePedido> _repoDetalle;
        private readonly IGenericRepository<Usuario> _repoUsuario;
        public ServicePedido(
            IGenericRepository<Pedido> repoPedido,
            IGenericRepository<DetallePedido> repoDetalle,
            IMapper mapper,
            ILogger<ServicePedido> logger,
            IGenericRepository<Usuario> repoUsuario)
        {
            _repoPedido = repoPedido;
            _repoDetalle = repoDetalle; 
            _mapper = mapper;
            _logger = logger;
            _repoUsuario = repoUsuario;
        }

        public async Task<ResponseApi> Create(PedidoDTO nuevoPedido)
        {
            try
            {
                if (nuevoPedido == null || nuevoPedido.Detalles == null || !nuevoPedido.Detalles.Any())
                {
                    _logger.LogWarning("Pedido o detalles nulos.");
                    return new ResponseApi("Los datos del pedido no pueden ser nulos.", false);
                }

              
                var pedidoEntidad = _mapper.Map<Pedido>(nuevoPedido);


                await _repoPedido.AddAsync(pedidoEntidad);


                foreach (var detalleDto in nuevoPedido.Detalles)
                {
                    var detalle = new DetallePedido
                    {
                        PedidoId = pedidoEntidad.PedidoId,
                        LibroId = detalleDto.LibroId,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = detalleDto.PrecioUnitario
                    };

                    await _repoDetalle.AddAsync(detalle); 
                }

                return new ResponseApi("El pedido y sus detalles fueron creados exitosamente.", true);
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

                // Eliminar detalles relacionados
                var detalles = await _repoDetalle.GetAllAsync(d => d.PedidoId == id);
                foreach (var detalle in detalles)
                {
                    await _repoDetalle.DeleteAsync(detalle.DetallePedidoId); // Ajusta el Id según tu modelo
                }

                // Ahora eliminar el pedido
                await _repoPedido.DeleteAsync(id);

                _logger.LogInformation("Pedido con ID: {Id} y sus detalles eliminados exitosamente.", id);
                return new ResponseApi("Pedido y detalles eliminados exitosamente.", true);
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

                var existente = await _repoPedido.GetByIdAsync(pedidoActualizar.PedidoID);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de actualizar pedido inexistente con ID: {Id}", pedidoActualizar.PedidoID);
                    return new ResponseApi("Pedido no encontrado.", false);
                }

                _mapper.Map(pedidoActualizar, existente);
                await _repoPedido.UpdateAsync(existente);
                _logger.LogInformation("Pedido actualizado exitosamente con ID: {Id}", pedidoActualizar.PedidoID);
                return new ResponseApi("Pedido actualizado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pedido con ID: {Id}", pedidoActualizar?.PedidoID);
                return new ResponseApi($"Error al actualizar el pedido. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<PedidoDTO>> GetByUsuarioId(int usuarioId)
        {
            try
            {
         
                var usuario = await _repoUsuario.GetByIdAsync(usuarioId);
               
                if (usuario == null)
                {
                    _logger.LogWarning("Usuario no encontrado con ID: {UsuarioId}", usuarioId);
                    return Enumerable.Empty<PedidoDTO>();
                }

                IEnumerable<Pedido> pedidos;

                if (!usuario.EsCliente)
                {
            
                    pedidos = await _repoPedido.GetAllAsync(p => p.UsuarioId == usuarioId);
                }
                else
                {
             
                    pedidos = await _repoPedido.GetAllAsync();
                }

                var pedidosDto = _mapper.Map<List<PedidoDTO>>(pedidos);

                foreach (var pedidoDto in pedidosDto)
                {
                    var detalles = await _repoDetalle.GetAllAsync(
                        d => d.PedidoId == pedidoDto.PedidoID,
                        d => d.Libro 
                    );

                    var detallesDto = _mapper.Map<List<DetallePedidoDTO>>(detalles);
                    pedidoDto.Detalles = detallesDto;
                }

                return pedidosDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedidos del usuario con ID: {UsuarioId}", usuarioId);
                return Enumerable.Empty<PedidoDTO>();
            }
        }


    
    }
}
