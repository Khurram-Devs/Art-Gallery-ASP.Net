namespace Art_Gallery.Models.DTOs
{
    public class ArtDIsplayModel
    {
        public IEnumerable<Art> Arts { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;

    }
}