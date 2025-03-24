using CertifyMe.Models;

namespace CertifyMe.Repositories
{
    public interface IUserRepository
    {
        Task<List<CourseCompletionRecord>> GetAllAsync();
        Task<CourseCompletionRecord?> GetByIdAsync(int id);
        Task CreateAsync(CourseCompletionRecord excelRow);
        Task UpsertFromExcelAsync(List<ExcelRowRecord> excelRows);
        Task UpdateAsync(CourseCompletionRecord excelRow);
        Task DeleteAsync(int id);
    }
}