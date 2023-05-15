using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Project.AssetIO
{
    public class JsonFileHandler : IFileHandler
    {
        private readonly JsonSerializerSettings settings;

        public JsonFileHandler(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }
        public JsonFileHandler()
        {
            this.settings = new JsonSerializerSettings();
        }
        async Task<T> ReadAsync<T>(string fullPath, JsonSerializerSettings settings)
        {
            if (!File.Exists(fullPath))
            {
                return default;
            }
            try{
                string jsonString = await File.ReadAllTextAsync(fullPath);
                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            catch{
                return default;
            }
        }
        public Task<T> ReadAsync<T>(string fullPath)
        {
            return this.ReadAsync<T>(fullPath, settings);
        }

        public Task WriteAsync<T>(T data, string fullPath, JsonSerializerSettings settings)
        {
            string jsonString = JsonConvert.SerializeObject(data, settings);
            if (!File.Exists(fullPath))
            {
                string folder = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(folder))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(folder);
                }
                using (File.Create(fullPath)) { }
            }
            return File.WriteAllTextAsync(fullPath, jsonString);
        }
        public Task WriteAsync<T>(T data, string fullPath)
        {
            return this.WriteAsync(data, fullPath, settings);
        }

        public T Read<T>(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return default;
            }
            string jsonString = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }

        public void Write<T>(T data, string fullPath)
        {
            string jsonString = JsonConvert.SerializeObject(data, settings);
            if (!File.Exists(fullPath))
            {
                string folder = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(folder))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(folder);
                }
                using (File.Create(fullPath)) { }
            }
            File.WriteAllText(fullPath, jsonString);
        }
    }
}