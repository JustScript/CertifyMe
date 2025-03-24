using CertifyMe.Models.Entities;

namespace CertifyMe.Services
{
    public interface IExcelService
    {
        Task<List<ExcelRowRecord>> GetRecordsFromExcelFileAsync(IFormFile file);
    }
}