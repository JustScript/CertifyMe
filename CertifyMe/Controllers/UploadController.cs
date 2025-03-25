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

    private readonly IImportExcelService _excelService;

    private readonly ITaskQueueService _backgroundTaskQueue;

    private readonly IServiceScopeFactory _scopeFactory;

    public UploadController(ILogger<UploadController> logger, IImportExcelService excelService, ITaskQueueService backgroundTaskQueue, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _excelService = excelService;
        _backgroundTaskQueue = backgroundTaskQueue;
        _scopeFactory = scopeFactory;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Upload excel file",
        Description = "Upload excel file")]
    public async Task<IActionResult> Post(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded or it is empty" });
        }

        List<ExcelRowRecord> records = await _excelService.GetRecordsFromExcelFileAsync(file);
        if (records.Any())
        {
            // Fire-and-forget task
            _backgroundTaskQueue.Enqueue(async token =>
            {
                using var scope = _scopeFactory.CreateScope();
                var courseCompletionRepository = scope.ServiceProvider.GetRequiredService<ICourseCompletionRepository>();

                await courseCompletionRepository.UpsertFromExcelAsync(records);
            });

            return Ok(new { message = $"File uploaded successfully" });
        }

        return BadRequest("No records found.");
    }
}
