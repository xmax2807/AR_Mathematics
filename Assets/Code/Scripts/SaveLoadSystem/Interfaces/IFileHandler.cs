namespace Project.AssetIO{
    public interface IFileHandler{
        T Read<T>(string fullPath);
        void Write<T>(T data, string fullPath);
    }
}