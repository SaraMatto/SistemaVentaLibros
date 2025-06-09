using System;
using System.Collections.Generic;

namespace SistemaABM_Libros_Data.Models;

public partial class Libro
{
    public int LibroId { get; set; }

    public int SubcategoriaId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public string? Isbn { get; set; }

    public string TipoLibro { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? Idioma { get; set; }

    public string? Editorial { get; set; }

    public int? AnioPublicacion { get; set; }

    public string? Descripcion { get; set; }

    public string? EstadoLibro { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Subcategoria Subcategoria { get; set; } = null!;
}
