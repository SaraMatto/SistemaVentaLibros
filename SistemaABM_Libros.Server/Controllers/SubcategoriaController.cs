using Microsoft.AspNetCore.Mvc;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;

namespace SistemaABM_Libros.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoriaController : ControllerBase
    {
        private readonly IServiceSubcategoria _servicioSubcategoria;
        private readonly ILogger<SubcategoriaController> _logger;

        public SubcategoriaController(IServiceSubcategoria servicioSubcategoria, ILogger<SubcategoriaController> logger)
        {
            _servicioSubcategoria = servicioSubcategoria;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetSubcategorias()
        {
            try
            {
                var subcategorias = await _servicioSubcategoria.GetAll();
                return Ok(subcategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las subcategorías.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubcategoriaDTO>> GetSubcategoria(int id)
        {
            try
            {
                var subcategoria = await _servicioSubcategoria.GetById(id);
                if (subcategoria == null)
                {
                    return NotFound(new ResponseApi($"Subcategoría con ID {id} no encontrada.", false));
                }
                return Ok(subcategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la subcategoría con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> CrearSubcategoria([FromBody] SubcategoriaDTO nuevaSubcategoriaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioSubcategoria.Create(nuevaSubcategoriaDto);
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
                _logger.LogError(ex, "Error al crear la subcategoría.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> PutSubcategoria([FromBody] SubcategoriaDTO subcategoriaActualizarDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi("Fallo de validación.", false));
                }

                var response = await _servicioSubcategoria.Update(subcategoriaActualizarDto);
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
                _logger.LogError(ex, $"Error al actualizar la subcategoría con ID {subcategoriaActualizarDto.Id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseApi>> DeleteSubcategoria(int id)
        {
            try
            {
                var response = await _servicioSubcategoria.Delete(id);
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
                _logger.LogError(ex, $"Error al eliminar la subcategoría con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }
    }
}
