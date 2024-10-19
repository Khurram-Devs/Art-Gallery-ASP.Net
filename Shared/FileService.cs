﻿namespace Art_Gallery.Shared
{
    public interface IFileService
    {
        void DeleteFile(string fileName);

        Task<string> SaveFile(IFormFile file, string[] allowedExten);

    }
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }



        public async Task<string> SaveFile(IFormFile file, string[] allowedExten)
        {
            var wwwPath = _environment.WebRootPath;
            var path = Path.Combine(wwwPath, "UploadImage");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var extension = Path.GetExtension(file.FileName);
            if (!allowedExten.Contains(extension))
            {
                throw new InvalidOperationException($"Only {string.Join(",", allowedExten)} files allowed");
            }
            string fileName = $"{Guid.NewGuid()}{extension}";
            string fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;

        }

        public void DeleteFile(string fileName)
        {
            var wwwPath = _environment.WebRootPath;
            var fileNameWithPath = Path.Combine(wwwPath, "UploadImage\\", fileName);
            if (!File.Exists(fileNameWithPath))
                throw new FileNotFoundException(fileName);
            File.Delete(fileNameWithPath);
        }


    }


}
