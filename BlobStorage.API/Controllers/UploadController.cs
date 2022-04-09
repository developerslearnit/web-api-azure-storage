using BlobStorage.API.Servies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlobStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly IAzureStorageService storageService;
        private readonly IConfiguration _config;

        public UploadController(IAzureStorageService storageService, IConfiguration config)
        {
            this.storageService = storageService;
            _config = config;
        }

        [HttpPost("azure")]
        public async Task<IActionResult> UploadFile()
        {
            if (!Request.Form.Files.Any())
            {
                return BadRequest("No files received from the upload");
            }

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var storageContainer = _config.GetSection("Azure:AzureStorageContainer").Value;

             var _uri =   await storageService.UploadAsync(file, storageContainer);
                return Ok(_uri);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload file");
        }
        

    }
}
