using UnityEngine;

namespace Project.AssetIO
{
    public static class Texture2DConfiguration
    {
        public const int MobileDimension = 1024;
        public static UnityEngine.Texture2D MobileConfig(UnityEngine.Texture2D origin)
        {
            RenderTexture rt = new(MobileDimension, MobileDimension, 0);
            Graphics.Blit(origin, rt);
            Texture2D result = new(MobileDimension, MobileDimension);
            result.ReadPixels(new Rect(0, 0, MobileDimension, MobileDimension), 0, 0);
            result.Compress(false);
            result.Apply();
            return result;
        }
    }
}