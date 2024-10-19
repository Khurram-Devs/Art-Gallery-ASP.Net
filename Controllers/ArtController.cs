using Art_Gallery.Models;
using Art_Gallery.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Art_Gallery.Controllers
{
    
    public class ArtController : Controller
    {
        private readonly IArtRepository _artRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IFileService _fileService;

        public ArtController(IArtRepository artRepo, IGenreRepository genreRepo, IFileService fileService)
        {
            _artRepo = artRepo;
            _genreRepo = genreRepo;
            _fileService = fileService;
        }
        public async Task<IActionResult> Arts()
        {
            var arts = await _artRepo.GetArts();
            return View(arts);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AddArt()
        {
            var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.GenreId.ToString()
            });

            ArtDTO artToAdd = new()
            {
                GenreList = genreSelectList
            };
            return View(artToAdd);

        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]

        public async Task<IActionResult> AddArt(ArtDTO artToAdd)
        {
            var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem { Text = genre.GenreName, Value = genre.GenreId.ToString() });
            artToAdd.GenreList = genreSelectList;

            //if (!ModelState.IsValid)
            //{
            //    return View(artToAdd);
            //}


            try
            {
                if (artToAdd.ImageFile != null)
                {
                    if (artToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file cannot exceed 1 MB");
                    }

                    string[] allowedExten = { ".jpeg", ".jpg", ".png" };
                    string imageName = await _fileService.SaveFile(artToAdd.ImageFile, allowedExten);
                    artToAdd.ArtImage = imageName;
                }

                Art art = new()
                {
                    ArtId = artToAdd.ArtId,
                    ArtName = artToAdd.ArtName,
                    ArtistName = artToAdd.ArtistName,
                    ArtImage = artToAdd.ArtImage,
                    GenreId = artToAdd.GenreId,
                    ArtPrice = artToAdd.ArtPrice,
                };

                await _artRepo.AddArt(art);
                TempData["successMsg"] = "Art added successfully";
                return RedirectToAction(nameof(AddArt));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View(artToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMsg"] = ex.Message;

                return View(artToAdd);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = "Error saving data";
                return View(artToAdd);
            }
        }
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateArt(int ArtId)
        {
            var art = await _artRepo.GetArtById(ArtId);
            if (art == null)
            {
                TempData["errorMsg"] = $"Art with the id: {ArtId} does not found";
                return RedirectToAction(nameof(Arts));
            }
            var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.GenreId.ToString(),
                Selected = genre.GenreId == art.GenreId
            });
            ArtDTO artToUpdate = new()
            {
                GenreList = genreSelectList,
                ArtName = art.ArtName,
                ArtistName = art.ArtistName,
                GenreId = art.GenreId,
                ArtPrice = art.ArtPrice,
                ArtImage = art.ArtImage,
            };
            return View(artToUpdate);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        public async Task<IActionResult> UpdateArt(ArtDTO artToUpdate)
        {
            var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.GenreId.ToString(),
                Selected = genre.GenreId == artToUpdate.GenreId
            });
            artToUpdate.GenreList = genreSelectList;

            if (!ModelState.IsValid)
                return View(artToUpdate);

            try
            {
                string oldImage = "";
                if (artToUpdate.ImageFile != null)
                {
                    if (artToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }

                    string[] allowedExten = { ".jpeg", ".jpg", ".png" };
                    string imageName = await _fileService.SaveFile(artToUpdate.ImageFile, allowedExten);

                    oldImage = artToUpdate.ArtImage;
                    artToUpdate.ArtImage = imageName;
                }

                Art art = new()
                {
                    ArtId = artToUpdate.ArtId,
                    ArtName = artToUpdate.ArtName,
                    ArtistName = artToUpdate.ArtistName,
                    ArtImage = artToUpdate.ArtImage,
                    GenreId = artToUpdate.GenreId,
                    ArtPrice = artToUpdate.ArtPrice,
                };
                await _artRepo.UpdateArt(art);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                TempData["successMsg"] = "Art is updated successfully";
                return RedirectToAction(nameof(Arts));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View(artToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMsg"] = ex.Message;
                return View(artToUpdate);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = "Error saving data";
                return View(artToUpdate);
            }


        }

        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteArt(int ArtId)
        {
            try
            {
                var art = await _artRepo.GetArtById(ArtId);
                if (art == null)
                {
                    TempData["errorMsg"] = $"Art with the id: {ArtId} does not found";
                }
                else
                {
                    await _artRepo.DeleteArt(art);
                    if (!string.IsNullOrEmpty(art.ArtImage))
                    {
                        _fileService.DeleteFile(art.ArtImage);
                    }

                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMsg"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMsg"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(Arts));
        }

    }
}
