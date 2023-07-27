using Gameframe.GUI.PanelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Screenshot{
    public class ScreenshotCaptureResultPanel : OkCancelPanelView{
        [SerializeField] private RawImage image;
        [SerializeField] private AspectRatioFitter.AspectMode mode = AspectRatioFitter.AspectMode.HeightControlsWidth;

        private AspectRatioFitter aspectRatioFitter;

        public void SetUI(Texture2D texture){
            aspectRatioFitter ??= image.GetComponent<AspectRatioFitter>();
            if(aspectRatioFitter == null){
                aspectRatioFitter = image.gameObject.AddComponent<AspectRatioFitter>();
            }
            aspectRatioFitter.aspectMode = mode;

            float ratio = (float)texture.width / texture.height;
            aspectRatioFitter.aspectRatio = ratio;

            image.texture = texture;
        }
    }
}