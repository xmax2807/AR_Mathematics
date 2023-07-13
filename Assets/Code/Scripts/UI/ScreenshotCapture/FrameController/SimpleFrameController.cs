using System;
using System.Collections;
using System.IO;
using Gameframe.GUI.PanelSystem;
using UnityEngine;

namespace Project.UI.Screenshot{
    public class SimpleFrameController : MonoBehaviour{
        [SerializeField] private SimpleFrameCaptureUI captureUI;
        [SerializeField] private PanelType screenCapturePanelType;
        [SerializeField] private PanelStackSystem stackSystem;
        private OkCancelPanelViewController m_controller;

        private Texture2D cacheTexture;

        public void OpenCaptureUI(){
            StartCoroutine(captureUI.ShowUI(OnFrameCaptured));
        }

        private void OnFrameCaptured(IFrame frame)
        {
            if(frame == null){
                return;
            }

            Debug.Log("Frame captured");
            StartCoroutine(ScreenshotCapture.CaptureScreenshotTexture(frame, OnCapturedScreen));
        }

        private void OnCapturedScreen(Texture2D texture2D)
        {
            cacheTexture = texture2D;
            ShowResultCapture();
        }

        private async void ShowResultCapture(){
            m_controller ??= new OkCancelPanelViewController(screenCapturePanelType, OnConfirmCallback);
            await stackSystem.PushAsync(m_controller);
            ScreenshotCaptureResultPanel resultPanel = m_controller.View as ScreenshotCaptureResultPanel;
            resultPanel.SetUI(cacheTexture);
        }

        private void OnConfirmCallback(bool shouldShare)
        {
            if(!shouldShare){
                Destroy(cacheTexture);
                return;
            }

            //StartCoroutine(StartPosting());
            StartCoroutine(StartSharing());
        }

        // private IEnumerator StartPosting(){
        //     yield return Managers.FacebookSDK.FacebookSDKManager.Instance.LogoutCurrentAccount();
        //     Managers.FacebookSDK.FacebookSDKManager.Instance.FeedController.PostFeed(cacheTexture.EncodeToPNG());
        // }

        private IEnumerator StartSharing(){
            string filePath = Path.Combine(Application.temporaryCachePath, "Screenshot.png");
            File.WriteAllBytes(filePath, cacheTexture.EncodeToPNG());
            new NativeShare().AddFile(filePath).SetText("asdasdasdsad").SetUrl("https://www.google.com").SetCallback(OnShareComplete).Share();
            yield break;
        }

        private void OnShareComplete(NativeShare.ShareResult result, string shareTarget)
        {
            Destroy(cacheTexture);
            Debug.Log(result.ToString());
            Debug.Log(shareTarget);
        }
    }
}