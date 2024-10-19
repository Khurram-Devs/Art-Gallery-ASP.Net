using Art_Gallery.Controllers;
using Art_Gallery.Models;
using Art_Gallery.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Art_Gallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly IReportRepository _reportRepo;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, IReportRepository reportRepo)
        {
            _homeRepository = homeRepository;
            _reportRepo = reportRepo;
            _logger = logger;
        }

        public async Task<ActionResult> Index(string sterm = "", int genreId = 0)
        {
            IEnumerable<Art> arts = await _homeRepository.GetArt(sterm, genreId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            var artDisplayModel = new ArtDIsplayModel
            {
                Arts = arts,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId,
            };

            DateTime startDate = DateTime.UtcNow.AddDays(-7);
            DateTime endDate = DateTime.UtcNow.AddDays(1);
            var topFiveSellingArts = await _reportRepo.GetTopNSellingArtsByDate(startDate, endDate);
            var vm = new TopNSoldArtsVm(startDate, endDate, topFiveSellingArts);
            var viewModel = new MultiViewModel
            {
                ArtDIsplayModel = artDisplayModel,
                TopNSoldArtsVm = vm
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
