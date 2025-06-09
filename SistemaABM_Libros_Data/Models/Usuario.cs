using System;
using System.Collections.Generic;

namespace SistemaABM_Libros_Data.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool EsCliente { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
