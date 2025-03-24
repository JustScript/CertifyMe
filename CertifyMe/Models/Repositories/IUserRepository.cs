using CertifyMe.Models.Entities;

namespace CertifyMe.Models.Repositories
{
    public interface IUserRepository
    {
        Task<List<CourseCompletionRecord>> GetAllAsync();
        Task<CourseCompletionRecord?> GetByIdAsync(int id);
        Task CreateAsync(CourseCompletionRecord excelRow);
        Task CreateOrUpdateByEmailAsync(List<ExcelRowRecord> excelRows);
        Task UpdateAsync(CourseCompletionRecord excelRow);
        Task DeleteAsync(int id);
    }
}