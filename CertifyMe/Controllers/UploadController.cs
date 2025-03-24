using CertifyMe.Models;
using CertifyMe.Repositories;
using CertifyMe.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CertifyMe.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;

    private readonly IUserRepository _userRepository;

    private readonly IExcelService _excelService;

    public UploadController(ILogger<UploadController> logger, IUserRepository userRepository, IExcelService excelService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _excelService = excelService;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Summary placeholder",
        Description = "Description placeholder")]
    public async Task<IActionResult> Post(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded or it is empty" });
        }

        List<ExcelRowRecord> records = await _excelService.GetRecordsFromExcelFileAsync(file);
        if (records.Any())
        {
            await _userRepository.UpsertFromExcelAsync(records);
            return Ok(new { message = $"File uploaded successfully" });
        }

        return BadRequest("No records found.");
    }
}
