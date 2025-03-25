using CertifyMe.Models;
using CertifyMe.Models.Entities;

namespace CertifyMe.Repositories
{
    public interface ICourseCompletionRepository
    {
        Task<List<CourseCompletionEntity>> GetAllAsync();

        Task<List<CourseCompletionEntity>> GetAllWithoutCertificateAsync();

        Task<List<CourseCompletionEntity>> GetAllWithCertificateNotSentAsync();

        Task<CourseCompletionEntity?> GetByIdAsync(int id);

        Task CreateAsync(CourseCompletionEntity excelRow);

        Task UpsertFromExcelAsync(List<ExcelRowRecord> excelRows);

        Task UpdateAsync(CourseCompletionEntity excelRow);

        Task DeleteAsync(int id);
    }
}