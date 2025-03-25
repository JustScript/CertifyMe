using CertifyMe.Models;
using CertifyMe.Models.Entities;

namespace CertifyMe.Repositories
{
    public interface ICourseCompletionRepository
    {
        Task<List<CourseCompletionEntity>> GetAllAsync();

        Task<CourseCompletionEntity?> GetByIdAsync(int id);

        Task<List<CourseCompletionEntity>> GetAllWithoutCertificateAsync();

        Task<List<CourseCompletionEntity>> GetAllWithCertificateNotSentAsync();

        Task UpsertFromExcelAsync(List<ExcelRowRecord> excelRows);

        Task CreateAsync(CourseCompletionEntity excelRow);

        Task UpdateAsync(CourseCompletionEntity excelRow);

        Task DeleteAsync(int id);

        Task<object> GetPagedAsync(int page, int pageSize);
    }
}