namespace WebApplication1.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        public string? CatalogCode { get; set; }

        public string? ProductCode { get; set; }

        public string? ProductName { get; set; }

        public string? Picture { get; set; }

        public double? UnitPrice { get; set; }

    }
}
