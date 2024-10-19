using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Art_Gallery.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepo;

        public GenreController(IGenreRepository genreRepo)
        {
            _genreRepo = genreRepo;
        }
        public async Task<IActionResult> Genres()
        {
            var genres = await _genreRepo.GetGenres();
            return View(genres);
        }

        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                var genreToAdd = new Genre
                {
                    GenreName = genre.GenreName,
                    GenreId = genre.GenreId,
                };
                await _genreRepo.AddGenre(genreToAdd);
                TempData["successMsg"] = "Genre Added Successfuly";
                return RedirectToAction(nameof(AddGenre));
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = "Genre could not added!";
                return View(genre);
            }

        }


        public async Task<IActionResult> UpdateGenre(int GenreId)
        {
            var genre = await _genreRepo.GetGenreById(GenreId);
            if (genre == null)
                throw new InvalidOperationException($"Genre with id: {GenreId} does not found");
            var genreToUpdate = new GenreDTO
            {
                GenreId = genre.GenreId,
                GenreName = genre.GenreName
            };
            return View(genreToUpdate);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDTO genreToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(genreToUpdate);
            }
            try
            {
                var genre = new Genre
                {
                    GenreName = genreToUpdate.GenreName,
                    GenreId = genreToUpdate.GenreId,
                };
                await _genreRepo.UpdateGenre(genre);
                TempData["successMsg"] = "Genre Updated Successfuly";
                return RedirectToAction(nameof(Genres));
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = "Genre could not Updated!";
                return View(genreToUpdate);
            }
        }



        public async Task<IActionResult> DeleteGenre(int GenreId)
        {
            var genre = await _genreRepo.GetGenreById(GenreId);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {GenreId} does not found");
            await _genreRepo.DeleteGenre(genre);
            return RedirectToAction(nameof(Genres));
        }

    }
}
