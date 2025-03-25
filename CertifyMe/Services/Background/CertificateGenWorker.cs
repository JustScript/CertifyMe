using CertifyMe.Extensions;
using CertifyMe.Models.Entities;
using CertifyMe.Repositories;
using HtmlRendererCore.PdfSharp;
using PdfSharpCore.Pdf;
using RazorLight;

namespace CertifyMe.Services
{
    public class CertificateGenWorker : BackgroundService
    {
        private readonly ILogger<CertificateGenWorker> _logger;

        private readonly IServiceProvider _serviceProvider;

        public CertificateGenWorker(ILogger<CertificateGenWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private async Task<byte[]?> GeneratePdf(CourseCompletionEntity model)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
                .UseMemoryCachingProvider()
                .Build();

            string html = await engine.CompileRenderAsync("CertificateTemplate.cshtml", model);

            PdfDocument pdf = PdfGenerator.GeneratePdf(html, PdfSharpCore.PageSize.A4);

            using var stream = new MemoryStream();
            pdf.Save(stream, false);
            return stream.ToArray();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background certificate generation worker started.");

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var courseCompletionRepository = scope.ServiceProvider.GetRequiredService<ICourseCompletionRepository>();

                    var withoutCertificates = await courseCompletionRepository.GetAllWithoutCertificateAsync();
                    if (!withoutCertificates.Any())
                    {
                        await Task.Delay(60000); // wait 1 minute if nothing to do
                    }

                    foreach (CourseCompletionEntity withoutCerificate in withoutCertificates)
                    {
                        var bytes = await GeneratePdf(withoutCerificate);
                        if (bytes != null)
                        {
                            await courseCompletionRepository.UpdateAsync(withoutCerificate.AttachCertificate($"certificate.pdf", bytes));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background certificate generation worker failed.");
                }
            }
        }
    }
}