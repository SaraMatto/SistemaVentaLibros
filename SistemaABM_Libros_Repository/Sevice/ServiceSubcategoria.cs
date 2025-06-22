using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServiceSubcategoria : IServiceSubcategoria
    {
        private readonly IGenericRepository<Subcategoria> _repoSubcategoria;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceSubcategoria> _logger;

        public ServiceSubcategoria(IGenericRepository<Subcategoria> repoSubcategoria, IMapper mapper, ILogger<ServiceSubcategoria> logger)
        {
            _repoSubcategoria = repoSubcategoria;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(SubcategoriaDTO nuevaSubcategoria)
        {
            try
            {
                if (nuevaSubcategoria == null)
                {
                    _logger.LogWarning("Método Crear llamado con SubcategoriaDTO nulo.");
                    return new ResponseApi("Los datos de la subcategoría no pueden ser nulos.", false);
                }

                var entidad = _mapper.Map<Subcategoria>(nuevaSubcategoria);
                await _repoSubcategoria.AddAsync(entidad);
                _logger.LogInformation("Subcategoría creada exitosamente: {Nombre}", nuevaSubcategoria.NombreSubcategoria);
                return new ResponseApi($"La subcategoría '{nuevaSubcategoria.NombreSubcategoria}' fue creada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la subcategoría: {Nombre}", nuevaSubcategoria?.NombreSubcategoria);
                return new ResponseApi($"Error al crear la subcategoría. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var existente = await _repoSubcategoria.GetByIdAsync(id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de eliminar subcategoría inexistente con ID: {Id}", id);
                    return new ResponseApi("Subcategoría no encontrada.", false);
                }

                await _repoSubcategoria.DeleteAsync(id);
                _logger.LogInformation("Subcategoría con ID: {Id} eliminada exitosamente.", id);
                return new ResponseApi("Subcategoría eliminada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la subcategoría con ID: {Id}", id);
                return new ResponseApi($"Error al eliminar la subcategoría. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<SubcategoriaDTO?>> GetAll()
        {
            try
            {
                var subcategorias = await _repoSubcategoria.GetAllAsync();
                return _mapper.Map<IEnumerable<SubcategoriaDTO>>(subcategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todas las subcategorías.");
                return Enumerable.Empty<SubcategoriaDTO>();
            }
        }

        public async Task<SubcategoriaDTO?> GetById(int id)
        {
            try
            {
                var subcategoria = await _repoSubcategoria.GetByIdAsync(id);
                return _mapper.Map<SubcategoriaDTO>(subcategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar subcategoría con ID: {Id}", id);
                return null;
            }
        }

        public async Task<ResponseApi> Update(SubcategoriaDTO subcategoriaActualizar)
        {
            try
            {
                if (subcategoriaActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con SubcategoriaDTO nulo.");
                    return new ResponseApi("Los datos de la subcategoría no pueden ser nulos.", false);
                }

                var existente = await _repoSubcategoria.GetByIdAsync(subcategoriaActualizar.Id);
                if (existente == null)
                {
                    _logger.LogWarning("Intento de actualizar subcategoría inexistente con ID: {Id}", subcategoriaActualizar.Id);
                    return new ResponseApi("Subcategoría no encontrada.", false);
                }

                _mapper.Map(subcategoriaActualizar, existente);
                await _repoSubcategoria.UpdateAsync(existente);
                _logger.LogInformation("Subcategoría actualizada exitosamente: {Nombre} con ID: {Id}", subcategoriaActualizar.NombreSubcategoria, subcategoriaActualizar.Id);
                return new ResponseApi($"La subcategoría '{subcategoriaActualizar.NombreSubcategoria}' fue actualizada exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la subcategoría: {Nombre} con ID: {Id}", subcategoriaActualizar?.NombreSubcategoria, subcategoriaActualizar?.Id);
                return new ResponseApi($"Error al actualizar la subcategoría. Detalles: {ex.Message}", false);
            }
        }
    }
}
