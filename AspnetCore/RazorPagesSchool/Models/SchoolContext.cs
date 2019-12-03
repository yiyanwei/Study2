using Microsoft.EntityFrameworkCore;

namespace RazorPagesSchool.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
            
        }

        public DbSet<Department> Departments{get;set;}
    }
}