using CertifyMe.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CertifyMe.Models.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CourseCompletionRecord> CourseCompletionRecords { get; set; }
    }
}

