using CertifyMe.Models;
using CertifyMe.Models.Entities;
using CertifyMe.Repositories;
using CertifyMe.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CertifyMe.Controllers;

[ApiController]
[Route("[controller]")]
public class GridController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;

    private readonly ICourseCompletionRepository _courseCompletionRepository;

    public GridController(ILogger<UploadController> logger, ICourseCompletionRepository courseCompletionRepository)
    {
        _logger = logger;
        _courseCompletionRepository = courseCompletionRepository;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "",
        Description = "")]
    public async Task<IActionResult> Get(int page, int pageSize)
    {
        var result = await _courseCompletionRepository.GetPagedAsync(page, pageSize);

        return Ok(result);
    }
}
