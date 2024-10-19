using Microsoft.EntityFrameworkCore;

namespace Art_Gallery.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Art>> GetArt(string sTerm="", int genreId=0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Art> arts = await (from art in _db.Arts 
                        join genre in _db.Genres
                        on art.GenreId equals genre.GenreId
                        join stock in _db.Stocks
                        on art.ArtId equals stock.ArtId
                        into art_stocks
                        from artWithStock in art_stocks.DefaultIfEmpty()
                        where string.IsNullOrWhiteSpace(sTerm) || (art!=null && art.ArtName.ToLower().StartsWith(sTerm))
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

            if (genreId > 0)
            {
                arts = arts.Where(a=>a.GenreId == genreId).ToList();
            }
            return arts;
        }
    }
}
