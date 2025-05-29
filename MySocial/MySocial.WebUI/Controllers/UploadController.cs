using Microsoft.AspNetCore.Mvc;
using MySocial.WebUI.ViewModel;

namespace MySocial.WebUI.Controllers
{
    public class UploadController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UploadController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("/Upload")]
        public IActionResult Index([FromForm] IFormFile image)
        {
            if (image == null)
            {
                return BadRequest(new { success = 0, message = "No file uploaded" });
            }

            var uniqueFileName = GetUniqueFileName(image.FileName);
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            image.CopyTo(new FileStream(filePath, FileMode.Create));

            return Ok(new
            {
                success = 1,
                file = new
                {
                    url = "/uploads/" + uniqueFileName
                }
            });
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
