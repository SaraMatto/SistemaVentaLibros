namespace SistemaABM_Libros_TranferObject.ModelsDTO
{
    public class PedidoDTO
    {
        public int PedidoID { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; }
        public string? EstadoPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public string? DireccionEnvio { get; set; }

        public List<DetallePedidoDTO> Detalles { get; set; } = new(); 
    }

}
