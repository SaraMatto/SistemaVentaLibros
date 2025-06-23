using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario _servicioUsuario;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IServiceUsuario servicioUsuario, ILogger<UsuarioController> logger)
        {
            _servicioUsuario = servicioUsuario;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario nuevoUsuario)
        {
            try
            {
                var response = await _servicioUsuario.Register(nuevoUsuario);
                if (response.Estado)
                    return Ok(new { mensaje = response.Mensaje });

                return BadRequest(new { error = response.Mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro.");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            try
            {
                var user = await _servicioUsuario.Login(loginDto.Email, loginDto.Password);
                if (user == null)
                    return Unauthorized(new { error = "Credenciales inválidas" });

                return Ok(new
                {
                    id = user.UsuarioId,
                    nombre = user.Nombre,
                    email = user.Email,
                    esCliente = user.EsCliente
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el login.");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _servicioUsuario.GetAll();
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
                var usuario = await _servicioUsuario.GetById(id);
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

                var response = await _servicioUsuario.Create(nuevoUsuarioDto);
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

                var response = await _servicioUsuario.Update(usuarioActualizarDto);
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
                var response = await _servicioUsuario.Delete(id);
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