using AutoMapper;
using SistemaABM_Libros_Data.Models;
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response;
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServicioUsuario : IServiceUsuario
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicioUsuario> _logger;

        public ServicioUsuario(IGenericRepository<Usuario> usuarioRepository, IMapper mapper, ILogger<ServicioUsuario> logger)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseApi> Create(UsuarioDTO nuevoUsuario)
        {
            try
            {
                if (nuevoUsuario == null)
                {
                    _logger.LogWarning("Método Crear llamado con UsuarioDTO nulo.");
                    return new ResponseApi("Los datos del usuario no pueden ser nulos.", false);
                }

                var usuarioEntidad = _mapper.Map<Usuario>(nuevoUsuario);
                await _usuarioRepository.AddAsync(usuarioEntidad);
                _logger.LogInformation("Usuario creado exitosamente: {UserName}", nuevoUsuario.Nombre);
                return new ResponseApi($"El usuario '{nuevoUsuario.Nombre}' fue creado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario: {UserName}", nuevoUsuario?.Nombre);
                return new ResponseApi($"Error al crear el usuario. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }

        public async Task<ResponseApi> Delete(int id)
        {
            try
            {
                var usuarioExistente = await _usuarioRepository.GetByIdAsync(id);
                if (usuarioExistente == null)
                {
                    _logger.LogWarning("Intento de eliminar usuario inexistente con ID: {UserId}", id);
                    return new ResponseApi("Usuario no encontrado.", false);
                }

                await _usuarioRepository.DeleteAsync(id);
                _logger.LogInformation("Usuario con ID: {UserId} eliminado exitosamente.", id);
                return new ResponseApi("Usuario eliminado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {UserId}", id);
                return new ResponseApi($"Error al eliminar el usuario. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAll()
        {
            try
            {
                var usuarios = await _usuarioRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los usuarios.");
                return Enumerable.Empty<UsuarioDTO>();
            }
        }

        public async Task<UsuarioDTO?> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar usuario con ID: {UserId}", id);
                return null;
            }
        }

        public async Task<ResponseApi> Register(Usuario usuario)
        {
            var existe = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (existe != null)
                return new ResponseApi("El correo ya está registrado", false);

            usuario.FechaRegistro = DateTime.Now;

            await _usuarioRepository.AddAsync(usuario);

            return new ResponseApi("Usuario registrado exitosamente", true);
        }

        public async Task<Usuario?> Login(string email, string password)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<ResponseApi> Update(UsuarioDTO usuarioActualizar)
        {
            try
            {
                if (usuarioActualizar == null)
                {
                    _logger.LogWarning("Método Actualizar llamado con UsuarioDTO nulo.");
                    return new ResponseApi("Los datos del usuario a actualizar no pueden ser nulos.", false);
                }

                var usuarioExistente = await _usuarioRepository.GetByIdAsync(usuarioActualizar.Id);

                if (usuarioExistente == null)
                {
                    _logger.LogWarning("Intento de actualizar usuario inexistente con ID: {UserId}", usuarioActualizar.Id);
                    return new ResponseApi("Usuario no encontrado.", false);
                }

                _mapper.Map(usuarioActualizar, usuarioExistente);
                await _usuarioRepository.UpdateAsync(usuarioExistente);
                _logger.LogInformation("Usuario actualizado exitosamente: {UserName} con ID: {UserId}", usuarioActualizar.Nombre, usuarioActualizar.Id);
                return new ResponseApi($"El usuario '{usuarioActualizar.Nombre}' fue actualizado exitosamente.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario: {UserName} con ID: {UserId}", usuarioActualizar?.Nombre, usuarioActualizar?.Id);
                return new ResponseApi($"Error al actualizar el usuario. Por favor, inténtelo de nuevo. Detalles: {ex.Message}", false);
            }
        }
    }
}