using Microsoft.EntityFrameworkCore;

namespace SchoolMinimalApi.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
    }
}
