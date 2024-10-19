using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required,MaxLength(20)]
        public string? StatusName { get; set; }
    }
}
