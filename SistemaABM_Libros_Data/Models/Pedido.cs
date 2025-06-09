using System;
using System.Collections.Generic;

namespace SistemaABM_Libros_Data.Models;

public partial class Pedido
{
    public int PedidoId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaPedido { get; set; }

    public string EstadoPedido { get; set; } = null!;

    public decimal TotalPedido { get; set; }

    public string? DireccionEnvio { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario Usuario { get; set; } = null!;
}
