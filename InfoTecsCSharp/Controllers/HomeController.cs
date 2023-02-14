using InfoTecsCSharp.Models;
using InfoTecsCSharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfoTecsCSharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HomeService _homeService;

        public HomeController(ILogger<HomeController> logger, HomeService service)
        {
            _logger = logger;
            _homeService = service;
        }

        [HttpGet]
        public async Task<IActionResult> ParseFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ParseFile(IFormFile file)
        {
            List<string> PermittedFileTypes = new List<string> {
                "text/csv",
                "application/csv",
            };

            if (!PermittedFileTypes.Contains(file.ContentType))
            {
                return BadRequest("Incorrect file type");
            }

            return await _homeService.ParseCsv(file, this);
        }

        [HttpGet]
        public IActionResult GetResults()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetResults([FromForm] GetResultFilter filter)
        {
            if (!ModelState.IsValid)
                return View(filter);

            return await _homeService.GetResults(filter, this);
        }


        [HttpGet]
        public IActionResult GetValuesByFilename()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetValuesByFilename([FromForm] string filename)
        {
            return await _homeService.GetValuesByFilename(filename, this);
        }
    }
}