using System.IO;
using System.Threading.Tasks;
using Domain;

namespace FileSystemImageStore
{
    public class FileSystemImageSaver : IImageSaver
    {
        private readonly string _folderPath;

        public FileSystemImageSaver(string folderPath)
        {
            _folderPath = folderPath;
        }

        public async Task SaveAsync(byte[] data, string name)
        {
            var path = Path.Combine(_folderPath, name);

            await using var fileStream = File.Create(path);

            fileStream.Write(data);
        }
    }
}
