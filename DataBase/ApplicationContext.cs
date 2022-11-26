using Microsoft.EntityFrameworkCore;


namespace testPR.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Article> Articles { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Test;Username=postgres;Password=159");
        }
    }
}