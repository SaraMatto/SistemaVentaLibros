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
        private readonly IServiceCategoria _repoCategoria;
        private readonly IServiceSubcategoria _repoSubCategoria;


        public LibroController(IServiceLibro servicioLibro, ILogger<LibroController> logger, IServiceCategoria repoCategoria, IServiceSubcategoria repoSubCategoria)
        {
            _servicioLibro = servicioLibro;
            _logger = logger;
            _repoCategoria = repoCategoria;
            _repoSubCategoria = repoSubCategoria;
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
        public async Task<ActionResult<ResponseApi>> CrearLibro([FromForm] LibroDTO formData)
        {
            try
            {
                string? fileName = null;

                if (formData.ImagenFile != null && formData.ImagenFile.Length > 0)
                {
                    var extension = Path.GetExtension(formData.ImagenFile.FileName);
                    fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formData.ImagenFile.CopyToAsync(stream);
                    }
                }

                var nuevoLibroDto = new LibroDTO
                {
                    Titulo = formData.Titulo,
                    Autor = formData.Autor,
                    ISBN = formData.ISBN,
                    TipoLibro = formData.TipoLibro,
                    Precio = formData.Precio,
                    Stock = formData.Stock,
                    Idioma = formData.Idioma,
                    Editorial = formData.Editorial,
                    AnioPublicacion = formData.AnioPublicacion,
                    Descripcion = formData.Descripcion,
                    EstadoLibro = formData.EstadoLibro,
                    SubcategoriaId = formData.SubcategoriaId,
                    Imagen = fileName // nombre del archivo guardado o null
                };

                // Si tienes servicio específico para creación, úsalo
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
        public async Task<ActionResult<ResponseApi>> PutLibro([FromForm] LibroDTO formData)
        {
            try
            {
                string? fileName = null;

                if (formData.ImagenFile != null && formData.ImagenFile.Length > 0)
                {
                    var extension = Path.GetExtension(formData.ImagenFile.FileName);
                    fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formData.ImagenFile.CopyToAsync(stream);
                    }

                    formData.Imagen = fileName; // Asigna el nuevo nombre si hay imagen
                }
                else
                {
                    // Si no hay archivo, deberías conservar la imagen actual
                    // Opcional: aquí puedes cargar la imagen actual del libro desde BD y asignarla a formData.Imagen
                    var libroExistente = await _servicioLibro.GetById(formData.Id);
                    if (libroExistente != null && string.IsNullOrEmpty(formData.Imagen))
                    {
                        formData.Imagen = libroExistente.Imagen;
                    }
                }

                var response = await _servicioLibro.Update(formData);

                return response.Estado ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el libro con ID {formData.Id}");
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

        // Obtener todas las categorías
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            try
            {
                var categorias = await _repoCategoria.GetAll();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        // Obtener una categoría por ID
        [HttpGet("categorias/{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoriaById(int id)
        {
            try
            {
                var categoria = await _repoCategoria.GetById(id);
                if (categoria == null)
                    return NotFound(new ResponseApi($"Categoría con ID {id} no encontrada.", false));

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la categoría con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        // Obtener todas las subcategorías
        [HttpGet("subcategorias")]
        public async Task<ActionResult<IEnumerable<SubcategoriaDTO>>> GetSubcategorias()
        {
            try
            {
                var subcategorias = await _repoSubCategoria.GetAll();
                return Ok(subcategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las subcategorías.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

        // Obtener una subcategoría por ID
        [HttpGet("subcategorias/{id}")]
        public async Task<ActionResult<SubcategoriaDTO>> GetSubcategoriaById(int id)
        {
            try
            {
                var subcategoria = await _repoSubCategoria.GetById(id);
                if (subcategoria == null)
                    return NotFound(new ResponseApi($"Subcategoría con ID {id} no encontrada.", false));

                return Ok(subcategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la subcategoría con ID {id}.");
                return StatusCode(500, new ResponseApi($"Error interno del servidor: {ex.Message}", false));
            }
        }

    }



}
