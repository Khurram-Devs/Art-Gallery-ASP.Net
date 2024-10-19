namespace Art_Gallery
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Art>> GetArt(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}