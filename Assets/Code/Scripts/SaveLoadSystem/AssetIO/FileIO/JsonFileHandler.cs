using System.IO;
using Newtonsoft.Json;
namespace Project.AssetIO{
    public class JsonFileHandler : IFileHandler
    {
        public T Read<T>(string fullPath)
        {
            if(!File.Exists(fullPath)){
                return default;
            }
            string jsonString = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public void Write<T>(T data, string fullPath)
        {
            string jsonString = JsonConvert.SerializeObject(data);
            if(!File.Exists(fullPath)){
                File.Create(fullPath);
            }
            File.WriteAllText(fullPath, jsonString);
        }
    }
}