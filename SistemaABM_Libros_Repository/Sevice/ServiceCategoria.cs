using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IGenericRepository<Categoria> _repoCategoria;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCategoria> _logger;

        public ServiceCategoria(IGenericRepository<Categoria> repoCategoria, IMapper mapper, ILogger<ServiceCategoria> logger)
        {
            _repoCategoria = repoCategoria;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(CategoriaDTO nuevaCategoria)
        {
            try
            {
                if (nuevaCategoria == null)
                {
                    _logger.LogWarning("Método Crear llamado con CategoriaDTO nulo.");
                    return new ResponseApi("Los datos de la categoría no pueden ser nulos.", false);
                }

                var categoriaEntidad = _mapper.Map<Categoria>(nuevaCategoria);
                await _repoCategoria.AddAsync(categoriaEntidad);
                _logger.LogInformation("Categoría creada exitosamente: {CategoryName}", nuevaCategoria.NombreCategoria);
                return new ResponseApi($"La categoría '{nuevaCategoria.NombreCategoria}' fue creada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoría: {CategoryName}", nuevaCategoria?.NombreCategoria);
                return new ResponseApi($"Error al crear la categoría. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var categoriaExistente = await _repoCategoria.GetByIdAsync(id);
                if (categoriaExistente == null)
                {
                    _logger.LogWarning("Intento de eliminar categoría inexistente con ID: {CategoryId}", id);
                    return new ResponseApi("Categoría no encontrada.", false);
                }

                await _repoCategoria.DeleteAsync(id);
                _logger.LogInformation("Categoría con ID: {CategoryId} eliminada exitosamente.", id);
                return new ResponseApi("Categoría eliminada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoría con ID: {CategoryId}", id);
                return new ResponseApi($"Error al eliminar la categoría. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<CategoriaDTO?>> GetAll()
        {
            try
            {
                var categorias = await _repoCategoria.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todas las categorías.");
                return Enumerable.Empty<CategoriaDTO>();
            }
        }

        public async Task<CategoriaDTO?> GetById(int id)
        {
            try
            {
                var categoria = await _repoCategoria.GetByIdAsync(id);
                return _mapper.Map<CategoriaDTO>(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar categoría con ID: {CategoryId}", id);
                return null;
            }
        }

        public async Task<ResponseApi> Update(CategoriaDTO categoriaActualizar)
        {
            try
            {
                if (categoriaActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con CategoriaDTO nulo.");
                    return new ResponseApi("Los datos de la categoría a actualizar no pueden ser nulos.", false);
                }

                var categoriaExistente = await _repoCategoria.GetByIdAsync(categoriaActualizar.Id);

                if (categoriaExistente == null)
                {
                    _logger.LogWarning("Intento de actualizar categoría inexistente con ID: {CategoryId}", categoriaActualizar.Id);
                    return new ResponseApi("Categoría no encontrada.", false);
                }

                _mapper.Map(categoriaActualizar, categoriaExistente);
                await _repoCategoria.UpdateAsync(categoriaExistente);
                _logger.LogInformation("Categoría actualizada exitosamente: {CategoryName} con ID: {CategoryId}", categoriaActualizar.NombreCategoria, categoriaActualizar.Id);
                return new ResponseApi($"La categoría '{categoriaActualizar.NombreCategoria}' fue actualizada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoría: {CategoryName} con ID: {CategoryId}", categoriaActualizar?.NombreCategoria, categoriaActualizar?.Id);
                return new ResponseApi($"Error al actualizar la categoría. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }
    }
}