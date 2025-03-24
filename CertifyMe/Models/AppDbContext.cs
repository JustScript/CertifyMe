using Microsoft.EntityFrameworkCore;

namespace CertifyMe.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CourseCompletionRecord> CourseCompletionRecords { get; set; }
    }
}

