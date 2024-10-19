namespace Art_Gallery.Models.DTOs
{
    public class StockDisplayModel
    {
        public int StockId { get; set; }
        public int ArtId { get; set; }
        public int Quantity { get; set; }
        public string? ArtName { get; set; }
    }
}
