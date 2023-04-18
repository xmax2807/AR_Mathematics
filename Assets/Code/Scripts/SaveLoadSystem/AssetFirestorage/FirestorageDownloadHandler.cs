using Firebase.Storage;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Project.AssetIO.Firebase
{
    public static class FirebaseStorageDownloadHandler
    {
        static FirebaseStorage storage => DatabaseManager.Storage;

        public static bool IsFileAvailableOffline(string relativePath)
        {
            string destinationPath = Path.Combine(UnityEngine.Application.persistentDataPath, relativePath);
            return System.IO.File.Exists(destinationPath);
        }
        public static bool TryGetLocalFilePath(System.Uri uri, out string fullFilePath){
            return TryGetLocalFilePath(uri.Segments[^1].Replace("%2F", "/"), out fullFilePath);
        }
        public static bool TryGetLocalFilePath(string relativePath, out string fullFilePath)
        {
            fullFilePath = Path.Combine(UnityEngine.Application.persistentDataPath, relativePath);
            bool result = System.IO.File.Exists(fullFilePath);
            new FileInfo(fullFilePath).Directory.Create();
            return result;
        }
        public static Task DownloadFile(string url, string fullFilePath)
        {
            var reference = storage.GetReferenceFromUrl(url);

            return reference.GetFileAsync(fullFilePath);
        }
        public static Task DownloadFile(StorageReference reference, string fullFilePath, System.Action<DownloadState> progressCallback = null)
        {

            return reference.GetFileAsync(
                    fullFilePath,
                    progressCallback == null ? null: new StorageProgress<DownloadState>(progressCallback),
                    CancellationToken.None
                );
        }
        public static StorageReference GetFileRef(string url) => storage.GetReferenceFromUrl(url);
    }
}