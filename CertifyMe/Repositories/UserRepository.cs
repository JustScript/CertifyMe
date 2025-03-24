using CertifyMe.Extensions;
using CertifyMe.Models;
using Microsoft.EntityFrameworkCore;

namespace CertifyMe.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseCompletionRecord>> GetAllAsync()
        {
            return await _context.CourseCompletionRecords.ToListAsync();
        }

        public async Task<CourseCompletionRecord?> GetByIdAsync(int id)
        {
            return await _context.CourseCompletionRecords.FindAsync(id);
        }

        public async Task CreateAsync(CourseCompletionRecord excelRow)
        {
            _context.CourseCompletionRecords.Add(excelRow);
            await _context.SaveChangesAsync();
        }

        public async Task UpsertFromExcelAsync(List<ExcelRowRecord> excelRows)
        {
            foreach (ExcelRowRecord excelRow in excelRows)
            {
                // Identify record by name, surname, email and course name
                CourseCompletionRecord? existingRecord = await _context.CourseCompletionRecords.FirstOrDefaultAsync(x => 
                    x.Name == excelRow.Name &&
                    x.Surname == excelRow.Surname &&
                    x.Email == excelRow.Email &&
                    x.CourseName == excelRow.CourseName
                );

                if (existingRecord == null)
                {
                    var newRecord = new CourseCompletionRecord().SyncWithExcelRow(excelRow);
                    await CreateAsync(newRecord);
                }
                else
                {
                    existingRecord = existingRecord.SyncWithExcelRow(excelRow);
                    await UpdateAsync(existingRecord);
                }
            }
        }

        public async Task UpdateAsync(CourseCompletionRecord excelRow)
        {
            _context.CourseCompletionRecords.Update(excelRow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.CourseCompletionRecords.FindAsync(id);
            if (user != null)
            {
                _context.CourseCompletionRecords.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}