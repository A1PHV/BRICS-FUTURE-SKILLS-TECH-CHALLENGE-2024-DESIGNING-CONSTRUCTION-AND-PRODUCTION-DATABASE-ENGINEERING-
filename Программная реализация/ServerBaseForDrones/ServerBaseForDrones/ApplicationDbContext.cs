using Microsoft.EntityFrameworkCore;

namespace ServerBaseForDrones
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Drone> Drones { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Здесь вы можете указать любые настройки модели, если нужно
            modelBuilder.Entity<Drone>().ToTable("Drones");
        }
    }
}
