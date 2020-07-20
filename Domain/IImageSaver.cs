using System.Threading.Tasks;

namespace Domain
{
    public interface IImageSaver
    {
        Task SaveAsync(byte[] data, string name);
    }
}