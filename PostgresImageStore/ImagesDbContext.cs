
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PostgresImageStore.Models;

namespace PostgresImageStore
{
    public class ImagesDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public ImagesDbContext(DbContextOptions<ImagesDbContext> options)
            : base(options)
        {
            
        }
    }

    public class ImagesDbContextFactory : IDesignTimeDbContextFactory<ImagesDbContext>
    {
        public ImagesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ImagesDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ozon-images;Username=postgres;Password=12345");

            return new ImagesDbContext(optionsBuilder.Options);
        }
    }
}
