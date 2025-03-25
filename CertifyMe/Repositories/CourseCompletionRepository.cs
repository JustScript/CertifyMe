using CertifyMe.Extensions;
using CertifyMe.Models;
using CertifyMe.Models.Database;
using CertifyMe.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertifyMe.Repositories
{
    public class CourseCompletionRepository : ICourseCompletionRepository
    {
        private readonly AppDbContext _context;

        public CourseCompletionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseCompletionEntity>> GetAllAsync()
        {
            return await _context.CourseCompletions.ToListAsync();
        }

        public async Task<CourseCompletionEntity?> GetByIdAsync(int id)
        {
            return await _context.CourseCompletions.FindAsync(id);
        }

        public async Task<List<CourseCompletionEntity>> GetAllWithoutCertificateAsync()
        {
            return await _context.CourseCompletions.Where(c => c.Certificate == null).ToListAsync();
        }

        public async Task<List<CourseCompletionEntity>> GetAllWithCertificateNotSentAsync()
        {
            return await _context.CourseCompletions.Where(c => c.Certificate != null && c.Certificate.IsCertificateSent == false).Include(c => c.Certificate).ToListAsync();
        }

        public async Task UpsertFromExcelAsync(List<ExcelRowRecord> excelRows)
        {
            foreach (ExcelRowRecord excelRow in excelRows)
            {
                // Identify record by name, surname, email and course name
                CourseCompletionEntity? existingRecord = await _context.CourseCompletions.FirstOrDefaultAsync(x =>
                    x.Name == excelRow.Name &&
                    x.Surname == excelRow.Surname &&
                    x.Email == excelRow.Email &&
                    x.CourseName == excelRow.CourseName
                );

                if (existingRecord == null)
                {
                    var newRecord = new CourseCompletionEntity().SyncWithExcelRow(excelRow);
                    await CreateAsync(newRecord);
                }
                else
                {
                    existingRecord = existingRecord.SyncWithExcelRow(excelRow);
                    await UpdateAsync(existingRecord);
                }
            }
        }

        public async Task CreateAsync(CourseCompletionEntity excelRow)
        {
            _context.CourseCompletions.Add(excelRow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseCompletionEntity excelRow)
        {
            _context.CourseCompletions.Update(excelRow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.CourseCompletions.FindAsync(id);
            if (user != null)
            {
                _context.CourseCompletions.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}