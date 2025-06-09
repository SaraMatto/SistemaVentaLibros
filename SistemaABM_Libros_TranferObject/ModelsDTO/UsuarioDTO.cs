using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaABM_Libros_TranferObject.ModelsDTO
{
    public class UsuarioDTO
    {
        public int Id{ get; set; }

        public string? Nombre { get; set; } = null!;

        public string? Apellido { get; set; } = null!;

        public string? Email { get; set; } = null!;

        public string? Telefono { get; set; }

        public bool EsCliente { get; set; }

        public DateOnly? FechaNacimiento { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public string? Contraseña { get; set; } = null!;

    }
}
