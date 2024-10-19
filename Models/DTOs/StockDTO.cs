using System.ComponentModel.DataAnnotations;

namespace Art_Gallery.Models.DTOs
{
    public class StockDTO
    {
        public int ArtId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }



    }
}
