using Microsoft.AspNetCore.Mvc;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IServicePedido _servicioPedido;
        private readonly IServiceDetallePedido _servicioDetallePedido;
        private readonly IServiceLibro _servicioLibro;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(
            IServicePedido servicioPedido,
            IServiceDetallePedido servicioDetallePedido,
            IServiceLibro servicioLibro,
            ILogger<PedidoController> logger)
        {
            _servicioPedido = servicioPedido;
            _servicioDetallePedido = servicioDetallePedido;
            _servicioLibro = servicioLibro;
            _logger = logger;
        }

        [HttpGet("{pedidoId}/detalles")]
        public async Task<ActionResult<IEnumerable<DetallePedidoDTO>>> GetDetalles(int pedidoId)
        {
            ActionResult<IEnumerable<DetallePedidoDTO>> result;
            try
            {
                var detalles = await _servicioDetallePedido.GetByPedidoId(pedidoId);
                result = Ok(detalles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles para pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpPost("{pedidoId}/detalles")]
        public async Task<ActionResult<ResponseApi>> AgregarDetalle(int pedidoId, [FromBody] DetallePedidoDTO nuevoDetalle)
        {
            ActionResult<ResponseApi> result;
            try
            {
                if (!ModelState.IsValid)
                {
                    result = BadRequest(new ResponseApi("Fallo de validación.", false));
                }
                else
                {
                    nuevoDetalle.PedidoId = pedidoId;
                    var response = await _servicioDetallePedido.Create(nuevoDetalle);

                    if (response.Estado)
                        result = Ok(response);
                    else
                        result = BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar detalle al pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpPut("{pedidoId}/detalles/{detalleId}")]
        public async Task<ActionResult<ResponseApi>> ActualizarDetalle(int pedidoId, int detalleId, [FromBody] DetallePedidoDTO detalleActualizar)
        {
            ActionResult<ResponseApi> result;
            try
            {
                if (!ModelState.IsValid)
                {
                    result = BadRequest(new ResponseApi("Fallo de validación.", false));
                }
                else if (detalleActualizar.Id != detalleId || detalleActualizar.PedidoId != pedidoId)
                {
                    result = BadRequest(new ResponseApi("El ID del detalle o el pedido no coinciden.", false));
                }
                else
                {
                    var response = await _servicioDetallePedido.Update(detalleActualizar);

                    if (response.Estado)
                        result = Ok(response);
                    else
                        result = NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar detalle {detalleId} del pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpDelete("{pedidoId}/detalles/{detalleId}")]
        public async Task<ActionResult<ResponseApi>> EliminarDetalle(int pedidoId, int detalleId)
        {
            ActionResult<ResponseApi> result;
            try
            {
                var response = await _servicioDetallePedido.Delete(detalleId);

                if (response.Estado)
                    result = Ok(response);
                else
                    result = NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar detalle {detalleId} del pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidosPorUsuario(int usuarioId)
        {
            ActionResult<IEnumerable<PedidoDTO>> result;
            try
            {
                var pedidos = await _servicioPedido.GetByUsuarioId(usuarioId);
                result = Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pedidos del usuario {usuarioId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> CrearPedido([FromBody] PedidoDTO nuevoPedido)
        {
            ActionResult<ResponseApi> result = null;
            ResponseApi response = null;
            string mensajeError = "";
            bool errorStock = false;

            try
            {
                if (!ModelState.IsValid)
                {
                    mensajeError = "Fallo de validación.";
                }
                else
                {
                    foreach (var detalle in nuevoPedido.Detalles)
                    {
                        var libro = await _servicioLibro.GetById(detalle.LibroId);
                        if (libro == null)
                        {
                            mensajeError = $"El libro con ID {detalle.LibroId} no existe.";
                            errorStock = true;
                            break;
                        }
                        if (detalle.Cantidad > libro.Stock)
                        {
                            mensajeError = $"Stock insuficiente para '{libro.Titulo}'. Disponible: {libro.Stock}, solicitado: {detalle.Cantidad}.";
                            errorStock = true;
                            break;
                        }
                    }

                    if (!errorStock)
                    {
                        foreach (var detalle in nuevoPedido.Detalles)
                        {
                            var libro = await _servicioLibro.GetById(detalle.LibroId);
                            libro.Stock -= detalle.Cantidad;
                            await _servicioLibro.Update(libro);
                        }

                        response = await _servicioPedido.Create(nuevoPedido);
                        if (response.Estado)
                        {
                            result = Ok(response);
                        }
                        else
                        {
                            result = BadRequest(response);
                        }
                    }
                    else
                    {
                        response = new ResponseApi(mensajeError, false);
                        result = BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido.");
                response = new ResponseApi($"Error interno: {ex.Message}", false);
                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpDelete("{pedidoId}")]
        public async Task<ActionResult<ResponseApi>> EliminarPedido(int pedidoId)
        {
            ActionResult<ResponseApi> result;
            try
            {
                var response = await _servicioPedido.Delete(pedidoId);
                if (response.Estado)
                    result = Ok(response);
                else
                    result = NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }

        [HttpPut("CambiarEstado/{pedidoId}")]
        public async Task<ActionResult<ResponseApi>> CambiarEstadoPedido(int pedidoId, [FromBody] string nuevoEstado)
        {
            ActionResult<ResponseApi> result;
            try
            {
                if (string.IsNullOrWhiteSpace(nuevoEstado))
                {
                    result = BadRequest(new ResponseApi("El nuevo estado no puede estar vacío.", false));
                }
                else
                {
                    var pedido = await _servicioPedido.GetById(pedidoId);
                    if (pedido == null)
                    {
                        result = NotFound(new ResponseApi("Pedido no encontrado.", false));
                    }
                    else
                    {
                        pedido.EstadoPedido = nuevoEstado;
                        var response = await _servicioPedido.Update(pedido);

                        if (response.Estado)
                            result = Ok(response);
                        else
                            result = BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar estado del pedido {pedidoId}.");
                result = StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
            return result;
        }
    }
}