using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Art_Gallery.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int GenreId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? GenreName { get; set; }

        public List<Art> Arts { get; set; }

    }
}
