using System;
using System.Collections.Generic;

namespace SistemaABM_Libros_Data.Models;

public partial class DetallePedido
{
    public int DetallePedidoId { get; set; }

    public int PedidoId { get; set; }

    public int LibroId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Libro Libro { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;
}
