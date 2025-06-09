using AutoMapper;
using SistemaABM_Libros_Data.Models; // Tu modelo de entidad Usuario
using SistemaABM_Libros_Data.Repository;
using SistemaABM_Libros_Data.Response; 
using SistemaABM_Libros_Repository.Interface;
using SistemaABM_Libros_TranferObject.ModelsDTO; 

namespace SistemaABM_Libros_Repository.Sevice
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public ServicioUsuario(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UsuarioDTO?>> ObtenerTodosUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
        }

        public async Task<UsuarioDTO?> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<ResponseApi> CrearUsuarioAsync(UsuarioDTO nuevoUsuarioDto)
        {
          
            try
            {
                var usuarioEntidad = _mapper.Map<Usuario>(nuevoUsuarioDto);
                await _usuarioRepository.AddAsync(usuarioEntidad);
                return new ResponseApi("Se Creo el usuario :" + nuevoUsuarioDto.Nombre, true);

            }
            catch (Exception ex)
            {
                return new ResponseApi("Error al crear el usuario :" + ex, false);
            }
        }

        public async Task<ResponseApi> ActualizarUsuarioAsync(UsuarioDTO usuarioActualizarDto) 
        {
            try
            {
                var usuarioExistente = await _usuarioRepository.GetByIdAsync(usuarioActualizarDto.Id);

                if (usuarioExistente == null)
                {
                    return new ResponseApi("El usuario no existe." , true);
                }

                _mapper.Map(usuarioActualizarDto, usuarioExistente);
                await _usuarioRepository.UpdateAsync(usuarioExistente);
                return new ResponseApi("Se Actualizo el usuario :" + usuarioActualizarDto.Nombre, true);
            }
            catch (Exception ex)
            {
                return new ResponseApi("Error al Actualizar el usuario :" + ex, false);
            }
         
        }

        public async Task<ResponseApi> EliminarUsuarioAsync(int id)
        {
            try
            {
                await _usuarioRepository.DeleteAsync(id);
                return new ResponseApi("Se elimino el usuario", true);
            }
            catch (Exception ex)
            {
                return new ResponseApi("Error al elimino el usuario :" + ex, false);
            }

        }

    }
}