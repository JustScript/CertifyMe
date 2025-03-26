using System.Text.RegularExpressions;
using CertifyMe.Extensions;
using CertifyMe.Models;
using CertifyMe.Models.Database;
using CertifyMe.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var completions = await _context.CourseCompletions.Where(c =>
                c.Name != null &&
                c.Surname != null &&
                c.CourseName != null &&
                c.Certificate == null
            ).ToListAsync();

            var filtered = completions
                .Where(c => Regex.IsMatch(c.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$") )
                .ToList();

            return filtered;
        }

        public async Task<List<CourseCompletionEntity>> GetAllWithCertificateNotSentAsync()
        {
            return await _context.CourseCompletions.Where(c => c.Certificate != null &&
            (
                c.Certificate.CertificateSendStatus == CertificateStatus.NotSent ||
                c.Certificate.CertificateSendStatus == CertificateStatus.Resend
            )).Include(c => c.Certificate).ToListAsync();
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
                    var newRecord = new CourseCompletionEntity(excelRow);
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

        public async Task<object> GetPagedAsync(int page, int pageSize)
        {
            var total = await _context.CourseCompletions.CountAsync();

            var data = await _context.CourseCompletions
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Email = c.Email,
                    CourseName = c.CourseName,
                    CompletionDate = c.CompletionDate.ToString("dd-MM-yyyy"),
                    CertificateStatus = c.Certificate != null ? c.Certificate.CertificateSendStatus.ToString() : "NotGenerated"
                })
                .ToListAsync();

            return new
            {
                Data = data,
                Total = total
            };
        }
    }
}