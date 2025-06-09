using System;
using System.Collections.Generic;

namespace SistemaABM_Libros_Data.Models;

public partial class Subcategoria
{
    public int SubcategoriaId { get; set; }

    public int CategoriaId { get; set; }

    public string NombreSubcategoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
