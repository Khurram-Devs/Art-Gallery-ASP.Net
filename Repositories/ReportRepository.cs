// File: Repositories/ReportRepository.cs
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Art_Gallery.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Art_Gallery.Repositories
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }




public async Task<IEnumerable<TopNSoldArtModel>> GetTopNSellingArtsByDate(DateTime startDate, DateTime endDate)
        {
            var topFiveSoldArts = await _dbContext.TopNSoldArtModels
                .FromSqlInterpolated($"EXEC Usp_GetTopNSellingArtsByDate {startDate.ToString("yyyy-MM-dd")}, {endDate.ToString("yyyy-MM-dd")}")
                .ToListAsync();

            return topFiveSoldArts.Any() ? topFiveSoldArts : new List<TopNSoldArtModel>();
        }







    }

    public interface IReportRepository
    {
        Task<IEnumerable<TopNSoldArtModel>> GetTopNSellingArtsByDate(DateTime startDate, DateTime endDate);
    }
}

