using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Managers
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance;

        public bool IsNetworkAvailable { get; private set; }
        private void Awake()
        {
            if (Instance == null) Instance = this;
            CheckInternetConnection();
        }

        public void CheckInternetConnection()
        {
            StartCoroutine(
                SendRequest("http://google.com", (request) =>
                {
                    Debug.Log(request.result);
                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log("error");
                        //TODO: Pop up message.
                    }
                })
            );
        }
        public async Task<bool> CheckInternetConnectionAsync(string url = "http://google.com")
        {
            UnityWebRequestAsyncOperation operation = SendRequest(url);
            while (!operation.isDone)
            {
                await Task.Delay(1);
            }
            return operation.webRequest.result != UnityWebRequest.Result.ConnectionError;
        }

        public IEnumerator SendRequest(string url, Action<UnityWebRequest> OnResult, int timeout = 30)
        {
            UnityWebRequest request = new(url)
            {
                timeout = timeout
            };
            yield return request.SendWebRequest();
            OnResult?.Invoke(request);
            request.Dispose();
        }
        private IEnumerator SendRequest(UnityWebRequest request, Action<UnityWebRequest> OnResult)
        {
            yield return request.SendWebRequest();
            OnResult?.Invoke(request);
            request.Dispose();
        }
        private UnityWebRequestAsyncOperation SendRequest(string url)
        {
            UnityWebRequest request = new(url);
            return request.SendWebRequest();
        }
        public void GetAudioClip(string url, Action<AudioClip> OnResult, AudioType type = AudioType.MPEG)
        {
            StartCoroutine(
                SendRequest(UnityWebRequestMultimedia.GetAudioClip(url, type),
                (req) =>
                {
                    if (req.result == UnityWebRequest.Result.Success)
                    {
                        OnResult?.Invoke(DownloadHandlerAudioClip.GetContent(req));
                    }
                    else{
                        Debug.Log(req.error);
                        OnResult?.Invoke(null);
                    }
                }));
        }

        public Coroutine GetAudioClipAsync(string url, Action<AudioClip> OnResult, AudioType type = AudioType.MPEG)
        {
            return StartCoroutine(
                SendRequest(UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG),
                (req) =>
                {
                    if (req.result == UnityWebRequest.Result.Success)
                    {
                        OnResult?.Invoke(DownloadHandlerAudioClip.GetContent(req));
                    }
                }));
        }

        public void RequestWithErrorHandling(Action action, Action<Exception> onError, int maxRetries = 3)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    action.Invoke();
                    break;
                }
                catch (Exception ex)
                {
                    // A network error occurred, retry the operation
                    if (i == maxRetries - 1)
                    {
                        // This was the last retry, call the onError callback
                        onError(ex);
                    }
                }
            }
        }

        public void RequestWithNotificationOnError(Action action, int maxRetries = 3){
            RequestWithErrorHandling(action, (error) => HandleRequestError(action, error), maxRetries);
        }

        private void HandleRequestError(Action request, Exception error){
            if(error is Firebase.FirebaseException firebaseException){
                
            }
        }
    }
}