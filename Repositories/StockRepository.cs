using Microsoft.EntityFrameworkCore;

namespace Art_Gallery.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<Stock?> GetStockByArtId(int artId) => await _dbContext.Stocks.FirstOrDefaultAsync(s => s.ArtId == artId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            var existingStock = await GetStockByArtId(stockToManage.ArtId);
            if (existingStock is null)
            {
                var stock = new Stock
                {
                    ArtId = stockToManage.ArtId,
                    Quantity = stockToManage.Quantity,
                };
                _dbContext.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from art in _dbContext.Arts
                                join stock in _dbContext.Stocks
                                on art.ArtId equals stock.ArtId
                                into art_stock
                                from artStock in art_stock.DefaultIfEmpty() where string.IsNullOrWhiteSpace(sTerm) || art.ArtName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    ArtId = art.ArtId,
                                    ArtName = art.ArtName,
                                    Quantity = artStock == null ? 0 : artStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }


    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByArtId(int artId);
        Task ManageStock(StockDTO stockToManage);

    }
}
