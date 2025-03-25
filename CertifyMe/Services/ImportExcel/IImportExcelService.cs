using CertifyMe.Models;

namespace CertifyMe.Services
{
    public interface IImportExcelService
    {
        Task<List<ExcelRowRecord>> GetRecordsFromExcelFileAsync(IFormFile file);
    }
}