using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int CartDetailId { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int ArtId { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; } 

        public Art Art { get; set; }
        public ShoppingCart ShoppingCart { get; set; }


    }
}
