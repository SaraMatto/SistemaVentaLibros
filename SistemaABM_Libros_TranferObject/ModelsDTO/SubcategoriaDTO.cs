namespace SistemaABM_Libros_TranferObject.ModelsDTO
{
    public class SubcategoriaDTO
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string? NombreSubcategoria { get; set; }
        public string? Descripcion { get; set; }
    }

}
