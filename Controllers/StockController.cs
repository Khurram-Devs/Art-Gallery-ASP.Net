using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Art_Gallery.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }
        public async Task<IActionResult> Stocks(string sTerm = "")
        {
            var stocks = await _stockRepo.GetStocks(sTerm);
            return View(stocks);
        }

        public async Task<IActionResult> ManageResult(int artId)
        {
            var existingStock = await _stockRepo.GetStockByArtId(artId);
            var stock = new StockDTO
            {
                ArtId = artId,
                Quantity = existingStock != null ? existingStock.Quantity : 0,
            };
            return View("ManageStock", stock); 
        }

        public async Task<IActionResult> ManageStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return View(stock);

            try
            {
                await _stockRepo.ManageStock(stock);
                TempData["successMsg"] = "Stock is updated successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                TempData["errorMsg"] = $"Something went wrong! Error: {ex.Message}";
                return View(stock); // Return the view with the error message
            }

            return RedirectToAction(nameof(Stocks));
        }



    }
}
