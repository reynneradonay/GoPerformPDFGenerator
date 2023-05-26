using GoPerformPDFGenerator.Models;
using GoPerformPDFGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoPerformPDFGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFGeneratorController : ControllerBase
    {
        private readonly ILogger<PDFGeneratorController> _logger;

        public readonly IWebHostEnvironment _environment;

        public PDFGeneratorController(ILogger<PDFGeneratorController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        [HttpPost]
        public ActionResult Generate([FromBody] PDFGeneratorViewModel viewModel)
        {
            PDFGenerator pdfGenerator = new(_environment);

            return File(pdfGenerator.Generate(viewModel.Deliverables, viewModel.KeyRoleOutcomes, viewModel.AssociateInfo), "application/pdf");
        }
    }
}
