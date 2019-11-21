using Microsoft.EntityFrameworkCore;
namespace RazorPagesMovie.Models
{
    public class RazorPagesMovieContext : DbContext
    {
        public RazorPagesMovieContext(DbContextOptions<RazorPagesMovieContext> options):base(options)
        {

        }

        public DbSet<Movie> Movies{get;set;} 
    }
}