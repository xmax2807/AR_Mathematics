using System.Collections.Generic;
using System.Threading.Tasks;
using Project.AssetIO.Firebase;

namespace Project.AssetIO{
    public class FileDataCacheHandler<T> : System.IDisposable{
        protected IFileHandler<T> _IFileHandler;
        protected Dictionary<string, T> cache;
        public FileDataCacheHandler(IFileHandler<T> fileHandler){
            _IFileHandler = fileHandler; 
            cache = new Dictionary<string, T>();
        }

        public void Dispose()
        {
            cache.Clear();
            System.GC.Collect();
        }

        public virtual async Task<T> GetFile(string filePath){
            if(cache.ContainsKey(filePath)) return cache[filePath];

            T result = await _IFileHandler.ReadAsync(filePath);
            
            if(result == null) return default;
            
            cache.Add(filePath,result);
            return result;
        }
    }

    public class FirestorageFileDataCacheHandler<T> : FileDataCacheHandler<T>
    {
        public FirestorageFileDataCacheHandler(IFileHandler<T> fileHandler) : base(fileHandler)
        {
        }

        public override async Task<T> GetFile(string url)
        {
            if(cache.ContainsKey(url)) return cache[url];

            var fileRef = FirebaseStorageDownloadHandler.GetFileRef(url);
            System.Uri uri = await fileRef.GetDownloadUrlAsync();

            bool isFileAvailableOffline = FirebaseStorageDownloadHandler.TryGetLocalFilePath(uri, out string fullFilePath);

            if (!isFileAvailableOffline)
            {
                await FirebaseStorageDownloadHandler.DownloadFile(fileRef, fullFilePath);
            }

            T result = await _IFileHandler.ReadAsync(fullFilePath);
            
            if(result == null) return default;

            cache.Add(url, result);
            return result;
        }
    }
}