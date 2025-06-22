using Microsoft.AspNetCore.Mvc;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : ControllerBase
    {
        private readonly IServiceLibro _servicioLibro;
        private readonly ILogger<LibroController> _logger;

        public LibroController(IServiceLibro servicioLibro, ILogger<LibroController> logger)
        {
            _servicioLibro = servicioLibro;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibros()
        {
            try
            {
                var libros = await _servicioLibro.GetAll();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los libros.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>> GetLibro(int id)
        {
            try
            {
                var libro = await _servicioLibro.GetById(id);
                if (libro == null)
                {
                    return NotFound(new ResponseApi($"Libro con ID {id} no encontrado.", false));
                }
                return Ok(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el libro con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> CrearLibro([FromBody] LibroDTO nuevoLibroDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioLibro.Create(nuevoLibroDto);
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
                _logger.LogError(ex, "Error al crear el libro.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> PutLibro([FromBody] LibroDTO libroActualizarDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioLibro.Update(libroActualizarDto);
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
                _logger.LogError(ex, $"Error al actualizar el libro con ID {libroActualizarDto.Id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseApi>> DeleteLibro(int id)
        {
            try
            {
                var response = await _servicioLibro.Delete(id);
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
                _logger.LogError(ex, $"Error al eliminar el libro con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }
    }
}
