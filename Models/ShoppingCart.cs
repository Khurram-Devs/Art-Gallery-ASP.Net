using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }

        [Required]
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }


    }
}
