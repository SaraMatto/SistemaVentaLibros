using Microsoft.AspNetCore.Mvc;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IServicePedido _servicioPedido;
        private readonly IServiceDetallePedido _servicioDetallePedido;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(IServicePedido servicioPedido, IServiceDetallePedido servicioDetallePedido, ILogger<PedidoController> logger)
        {
            _servicioPedido = servicioPedido;
            _servicioDetallePedido = servicioDetallePedido;
            _logger = logger;
        }


        [HttpGet("{pedidoId}/detalles")]
        public async Task<ActionResult<IEnumerable<DetallePedidoDTO>>> GetDetalles(int pedidoId)
        {
            try
            {
                var detalles = await _servicioDetallePedido.GetByPedidoId(pedidoId);
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles para pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPost("{pedidoId}/detalles")]
        public async Task<ActionResult<ResponseApi>> AgregarDetalle(int pedidoId, [FromBody] DetallePedidoDTO nuevoDetalle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseApi("Fallo de validación.", false));

                nuevoDetalle.PedidoId = pedidoId;
                var response = await _servicioDetallePedido.Create(nuevoDetalle);

                if (response.Estado)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar detalle al pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPut("{pedidoId}/detalles/{detalleId}")]
        public async Task<ActionResult<ResponseApi>> ActualizarDetalle(int pedidoId, int detalleId, [FromBody] DetallePedidoDTO detalleActualizar)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseApi("Fallo de validación.", false));

                if (detalleActualizar.Id != detalleId || detalleActualizar.PedidoId != pedidoId)
                    return BadRequest(new ResponseApi("El ID del detalle o el pedido no coinciden.", false));

                var response = await _servicioDetallePedido.Update(detalleActualizar);

                if (response.Estado)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar detalle {detalleId} del pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpDelete("{pedidoId}/detalles/{detalleId}")]
        public async Task<ActionResult<ResponseApi>> EliminarDetalle(int pedidoId, int detalleId)
        {
            try
            {
                var response = await _servicioDetallePedido.Delete(detalleId);

                if (response.Estado)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar detalle {detalleId} del pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidosPorUsuario(int usuarioId)
        {
            try
            {
                var pedidos = await _servicioPedido.GetByUsuarioId(usuarioId);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pedidos del usuario {usuarioId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> CrearPedido([FromBody] PedidoDTO nuevoPedido)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseApi("Fallo de validación.", false));

                var response = await _servicioPedido.Create(nuevoPedido);

                if (response.Estado)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido.");
                return StatusCode(500, new ResponseApi($"Error interno: {ex.Message}", false));
            }
        }

        [HttpDelete("{pedidoId}")]
        public async Task<ActionResult<ResponseApi>> EliminarPedido(int pedidoId)
        {
            try
            {
                var response = await _servicioPedido.Delete(pedidoId);
                if (response.Estado)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPut("CambiarEstado/{pedidoId}")]
        public async Task<ActionResult<ResponseApi>> CambiarEstadoPedido(int pedidoId, [FromBody] string nuevoEstado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nuevoEstado))
                    return BadRequest(new ResponseApi("El nuevo estado no puede estar vacío.", false));

                var pedido = await _servicioPedido.GetById(pedidoId);
                if (pedido == null)
                    return NotFound(new ResponseApi("Pedido no encontrado.", false));

                pedido.EstadoPedido = nuevoEstado;

                var response = await _servicioPedido.Update(pedido);

                if (response.Estado)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar estado del pedido {pedidoId}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

    }
}
