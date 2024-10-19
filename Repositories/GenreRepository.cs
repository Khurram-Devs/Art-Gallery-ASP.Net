using Microsoft.EntityFrameworkCore;

namespace Art_Gallery.Repositories
{
    public interface IGenreRepository
    {
        Task AddGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task<Genre?> GetGenreById(int GenreId);
        Task DeleteGenre(Genre genre);
        Task<IEnumerable<Genre>> GetGenres();

    }
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddGenre(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGenre(Genre genre)
        {
            _dbContext.Genres.Update(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGenre(Genre genre)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Genre?> GetGenreById(int GenreId)
        {
            return await _dbContext.Genres.FindAsync(GenreId);
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _dbContext.Genres.ToListAsync();
        }

       
    }


}
