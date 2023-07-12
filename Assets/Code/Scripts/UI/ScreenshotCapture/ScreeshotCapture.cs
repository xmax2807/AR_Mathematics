using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Screenshot
{
    public static class ScreenshotCapture
    {
        public static IEnumerator CaptureScreenshotTexture(IFrame frame, System.Action<Texture2D> OnCapturedScreen)
        {
            Canvas canvas = Managers.GameManager.RootCanvas;
            Rect canvasRect = canvas.GetComponent<RectTransform>().rect;
            // Get the position and dimensions from the RectTransform
            Vector2 capturePosition = frame.GetPosition();
            Vector2 captureSize = frame.GetSize() * canvas.scaleFactor;
            //capturePosition /= canvas.scaleFactor;

            Debug.Log(capturePosition);
            Debug.Log(captureSize);

            // Set the capture width and height
            int captureWidth = Mathf.Min(Mathf.RoundToInt(captureSize.x), (int)canvasRect.width);
            int captureHeight = Mathf.Min(Mathf.RoundToInt(captureSize.y), Screen.height);

            // Create a render texture with the desired size
            RenderTexture renderTexture = RenderTexture.GetTemporary(captureWidth, captureWidth, 24);

            // Set the active render texture
            RenderTexture.active = renderTexture;

            // Create a new texture
            Texture2D screenshotTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

            // Capture the screenshot with the custom position
            int capturePosX = Mathf.RoundToInt(capturePosition.x - captureWidth / 2);
            int capturePosY = Mathf.RoundToInt(capturePosition.y - captureHeight / 2);

            Debug.Log(new Vector2(capturePosX, capturePosY));
            
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);

            //read the pixels from the render texture
            screenshotTexture.ReadPixels(new Rect(capturePosX, capturePosY, captureWidth, captureHeight), 0, 0);
            screenshotTexture.Apply();
            OnCapturedScreen?.Invoke(screenshotTexture);
        }
    }
}