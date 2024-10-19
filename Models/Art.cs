using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("Art")]
    public class Art
    {
        public int ArtId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ArtName { get; set; }
        [Required]
        [MaxLength(40)]
        public string? ArtistName { get; set; }
        [Required]
        public double ArtPrice { get; set; }
        [Required]
        public string? ArtImage { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<CartDetail> CartDetails{ get; set; }

        public Stock Stock { get; set; }

        [NotMapped]
        public string GenreName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }

    }
}
