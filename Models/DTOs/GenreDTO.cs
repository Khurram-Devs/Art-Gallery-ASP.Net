using System.ComponentModel.DataAnnotations;

namespace Art_Gallery.Models.DTOs
{
    public class GenreDTO
    {
        public int GenreId { get; set; }

        [Required]
        [MaxLength(40)]
        public string GenreName { get; set; }
    }
}
