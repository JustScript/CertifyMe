using CertifyMe.Models.Database;
using CertifyMe.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertifyMe.Models.Repositories
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

        public async Task CreateOrUpdateByEmailAsync(List<ExcelRowRecord> excelRows)
        {
            foreach (ExcelRowRecord row in excelRows)
            {
                var existingUser = await _context.CourseCompletionRecords.FirstOrDefaultAsync(u => u.Email == row.Email);
                if (existingUser == null)
                {
                    await CreateAsync(CourseCompletionRecord.FromDto(row));
                }
                else
                {
                    await UpdateAsync(CourseCompletionRecord.FromDto(row));
                }
            }

            await _context.SaveChangesAsync();
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