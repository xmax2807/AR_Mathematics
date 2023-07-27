using System.Threading.Tasks;

namespace Project.AssetIO{
    public interface IFileHandler{
        T Read<T>(string fullPath);
        Task<T> ReadAsync<T>(string fullPath);
        void Write<T>(T data, string fullPath);
        Task WriteAsync<T>(T data, string fullPath);
    }
    public interface IFileHandler<T>{
        Task<T> ReadAsync(string fullPath);
        Task WriteAsync(T data, string fullPath);
    }
}