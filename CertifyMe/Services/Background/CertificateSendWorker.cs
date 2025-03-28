using System.Net;
using System.Net.Mail;
using CertifyMe.Extensions;
using CertifyMe.Models;
using CertifyMe.Models.Entities;
using CertifyMe.Repositories;
using HtmlRendererCore.PdfSharp;
using PdfSharpCore.Pdf;
using RazorLight;

namespace CertifyMe.Services
{
    public class CertificateSendWorker : BackgroundService
    {
        private readonly ILogger<CertificateGenWorker> _logger;

        private readonly IServiceProvider _serviceProvider;

        public CertificateSendWorker(ILogger<CertificateGenWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background certificate email send worker started.");

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var courseCompletionRepository = scope.ServiceProvider.GetRequiredService<ICourseCompletionRepository>();

                    var unsentCertificates = await courseCompletionRepository.GetAllWithCertificateNotSentAsync();
                    if (!unsentCertificates.Any())
                    {
                        await Task.Delay(60000); // wait 1 minute if nothing to do
                    }

                    foreach (CourseCompletionEntity unsentCertificate in unsentCertificates)
                    {
                        if (unsentCertificate.Certificate != null)
                        {
                            try
                            {
                                unsentCertificate.SendCertificateByEmail();
                                unsentCertificate.Certificate.CertificateSendStatus = CertificateStatus.Sent;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation($"Background certificate email send worker failed. {ex.Message}");

                                unsentCertificate.Certificate.CertificateSendStatus = CertificateStatus.Failed;
                            }
                            finally
                            {
                                await courseCompletionRepository.UpdateAsync(unsentCertificate);
                                await Task.Delay(1000);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background certificate email send worker failed.");
                }
            }
        }
    }
}