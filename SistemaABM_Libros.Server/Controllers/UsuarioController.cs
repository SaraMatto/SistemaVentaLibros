using Microsoft.AspNetCore.Mvc;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IServicioUsuario servicioUsuario, ILogger<UsuarioController> logger)
        {
            _servicioUsuario = servicioUsuario;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _servicioUsuario.ObtenerTodosUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _servicioUsuario.ObtenerUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new ResponseApi($"Usuario con ID {id} no encontrado.", false));
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> CrearUsuario([FromBody] UsuarioDTO nuevoUsuarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioUsuario.CrearUsuarioAsync(nuevoUsuarioDto);
                if (response.Estado)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> PutUsuario([FromBody] UsuarioDTO usuarioActualizarDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioUsuario.ActualizarUsuarioAsync(usuarioActualizarDto);
                if (response.Estado)
                {
                    return Ok(response);
                }
                else
                {
                 return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el usuario con ID {usuarioActualizarDto.Id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseApi>> DeleteUsuario(int id)
        {
            try
            {
                var response = await _servicioUsuario.EliminarUsuarioAsync(id);
                if (response.Estado)
                {
                    return Ok(response);
                }
                else
                {

                 return NotFound(response);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el usuario con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }
    }
}