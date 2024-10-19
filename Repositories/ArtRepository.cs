using Art_Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Art_Gallery.Repositories
{
    public interface IArtRepository
    {
        Task AddArt(Art art);
        Task DeleteArt(Art art);
        Task<Art> GetArtById(int Artid);
        Task<IEnumerable<Art>> GetArts();
        Task UpdateArt(Art art);

    }

    public class ArtRepository : IArtRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ArtRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddArt(Art art)
        {
            _dbContext.Arts.Add(art);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateArt(Art art)
        {
            _dbContext.Arts.Update(art);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteArt(Art art)
        {
            _dbContext.Arts.Remove(art);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Art> GetArtById(int ArtId) => await _dbContext.Arts.FindAsync(ArtId);

        public async Task<IEnumerable<Art>> GetArts() => await(from art in _dbContext.Arts
        join genre in _dbContext.Genres
        on art.GenreId equals genre.GenreId
                        join stock in _dbContext.Stocks
        on art.ArtId equals stock.ArtId
        into art_stocks
                        from artWithStock in art_stocks.DefaultIfEmpty()
                        where  (art!= null)
                        select new Art
                        { 
                            ArtId = art.ArtId,
                            ArtImage = art.ArtImage,
                            ArtistName = art.ArtistName,
                            ArtName = art.ArtName,
                            GenreId = art.GenreId,
                            ArtPrice = art.ArtPrice,
                            GenreName = genre.GenreName,
                            Quantity = artWithStock == null ? 0 : artWithStock.Quantity,
                        }
                        ).ToListAsync();


}


}
