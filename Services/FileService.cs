using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GLMS.Web.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SavePdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".pdf")
                throw new Exception("Only PDF files are allowed.");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "contracts");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + ".pdf";

            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}