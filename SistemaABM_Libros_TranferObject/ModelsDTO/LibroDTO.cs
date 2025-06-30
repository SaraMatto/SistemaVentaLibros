using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaABM_Libros_TranferObject.ModelsDTO
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public int SubcategoriaId { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? ISBN { get; set; }
        public string? TipoLibro { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Idioma { get; set; }
        public string? Editorial { get; set; }
        public int? AnioPublicacion { get; set; }
        public string? Descripcion { get; set; }
        public string? EstadoLibro { get; set; }
        public string? Imagen { get; set; }
        public IFormFile? ImagenFile { get; set; }
        public SubcategoriaDTO? Subcategoria { get; set; } 
    }

}
