namespace SistemaABM_Libros_TranferObject.ModelsDTO
{
    public class DetallePedidoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int LibroId { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public string? TituloLibro { get; set; }

        public LibroDTO? Libro { get; set; } 
    }


}
