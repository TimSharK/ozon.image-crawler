using System.Threading.Tasks;
using Domain;
using PostgresImageStore.Models;

namespace PostgresImageStore
{
    public class PostgresImageSaver : IImageSaver
    {
        private readonly ImagesDbContext _dbContext;

        public PostgresImageSaver(ImagesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(byte[] data, string name)
        {
            var img = new Image
            {
                Name = name,
                Data = data
            };

            await _dbContext.Images.AddAsync(img);
            await _dbContext.SaveChangesAsync();
        }
    }
}
