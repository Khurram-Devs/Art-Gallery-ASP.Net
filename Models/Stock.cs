using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int StockId { get; set; }
        public int ArtId { get; set; }
        public int Quantity { get; set; }

        public Art? Art { get; set; }
    }
}
