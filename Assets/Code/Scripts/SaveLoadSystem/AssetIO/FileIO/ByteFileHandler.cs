using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.AssetIO
{
    public class TextureFileHandler : IFileHandler<UnityEngine.Texture2D>
    {
        public async Task<Texture2D> ReadAsync(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return null;
            }

            try
            {
                string ext = Path.GetExtension(fullPath).ToLower();
                byte[] loadData = await File.ReadAllBytesAsync(fullPath);

                Texture2D origin = new(1024, 1024, ext == ".png" ? TextureFormat.ARGB32 : TextureFormat.RGB24, false);
                origin.LoadImage(loadData);

                if (origin.width > Texture2DConfiguration.MobileDimension || origin.height > Texture2DConfiguration.MobileDimension)
                {
                    origin = Texture2DConfiguration.MobileConfig(origin);
                    await WriteAsync(origin, fullPath);
                }

                return origin;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task WriteAsync(UnityEngine.Texture2D data, string fullPath)
        {
            byte[] bytesArray = data.GetRawTextureData();
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath);
            }
            await File.WriteAllBytesAsync(fullPath, bytesArray);
        }
    }
}