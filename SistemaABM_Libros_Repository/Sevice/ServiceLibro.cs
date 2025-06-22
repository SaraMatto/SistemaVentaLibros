using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServiceLibro : IServiceLibro
    {
        private readonly IGenericRepository<Libro> _repoLibro;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceLibro> _logger;

        public ServiceLibro(IGenericRepository<Libro> repoLibro, IMapper mapper, ILogger<ServiceLibro> logger)
        {
            _repoLibro = repoLibro;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(LibroDTO nuevoLibro)
        {
            try
            {
                if (nuevoLibro == null)
                {
                    _logger.LogWarning("Método Crear llamado con LibroDTO nulo.");
                    return new ResponseApi("Los datos del libro no pueden ser nulos.", false);
                }

                var entidad = _mapper.Map<Libro>(nuevoLibro);
                await _repoLibro.AddAsync(entidad);
                _logger.LogInformation("Libro creado exitosamente: {Titulo}", nuevoLibro.Titulo);
                return new ResponseApi($"El libro '{nuevoLibro.Titulo}' fue creado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el libro: {Titulo}", nuevoLibro?.Titulo);
                return new ResponseApi($"Error al crear el libro. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var existente = await _repoLibro.GetByIdAsync(id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de eliminar libro inexistente con ID: {Id}", id);
                    return new ResponseApi("Libro no encontrado.", false);
                }

                await _repoLibro.DeleteAsync(id);
                _logger.LogInformation("Libro con ID: {Id} eliminado exitosamente.", id);
                return new ResponseApi("Libro eliminado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el libro con ID: {Id}", id);
                return new ResponseApi($"Error al eliminar el libro. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<LibroDTO?>> GetAll()
        {
            try
            {
                var libros = await _repoLibro.GetAllAsync();
                return _mapper.Map<IEnumerable<LibroDTO>>(libros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los libros.");
                return Enumerable.Empty<LibroDTO>();
            }
        }

        public async Task<LibroDTO?> GetById(int id)
        {
            try
            {
                var libro = await _repoLibro.GetByIdAsync(id);
                return _mapper.Map<LibroDTO>(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar libro con ID: {Id}", id);
                return null;
            }
        }

        public async Task<ResponseApi> Update(LibroDTO libroActualizar)
        {
            try
            {
                if (libroActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con LibroDTO nulo.");
                    return new ResponseApi("Los datos del libro no pueden ser nulos.", false);
                }

                var existente = await _repoLibro.GetByIdAsync(libroActualizar.Id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de actualizar libro inexistente con ID: {Id}", libroActualizar.Id);
                    return new ResponseApi("Libro no encontrado.", false);
                }

                _mapper.Map(libroActualizar, existente);
                await _repoLibro.UpdateAsync(existente);
                _logger.LogInformation("Libro actualizado exitosamente: {Titulo} con ID: {Id}", libroActualizar.Titulo, libroActualizar.Id);
                return new ResponseApi($"El libro '{libroActualizar.Titulo}' fue actualizado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro: {Titulo} con ID: {Id}", libroActualizar?.Titulo, libroActualizar?.Id);
                return new ResponseApi($"Error al actualizar el libro. Detalles: {ex.Message}", false);
            }
        }
    }
}
