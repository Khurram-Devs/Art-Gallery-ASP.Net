using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this using directive

namespace Art_Gallery.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepo;
        private readonly ILogger<ReportController> _logger; // Add this field

        public ReportController(IReportRepository reportRepo, ILogger<ReportController> logger)
        {
            _reportRepo = reportRepo;
            _logger = logger; // Initialize the logger
        }

        [HttpGet]
        public async Task<ActionResult> TopFiveSellingArts(DateTime? sDate = null, DateTime? eDate = null)
        {
            try
            {
                DateTime startDate = sDate ?? DateTime.UtcNow.AddDays(-7);
                DateTime endDate = eDate ?? DateTime.UtcNow;

                var topFiveSellingArts = await _reportRepo.GetTopNSellingArtsByDate(startDate, endDate);

                var vm = new TopNSoldArtsVm(startDate, endDate, topFiveSellingArts);
                return View(vm);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An error occurred while retrieving top-selling arts.");
                TempData["errorMsg"] = "Something went wrong: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
