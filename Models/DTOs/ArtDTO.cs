using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Art_Gallery.Models.DTOs
{
    public class ArtDTO
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
        [Required]
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }

    }
}
