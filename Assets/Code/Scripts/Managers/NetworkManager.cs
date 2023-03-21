using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Managers
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance;
        private void Awake()
        {
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckInternetConnection();
        }
        
        public void CheckInternetConnection()
        {
            StartCoroutine(
                SendRequest("http://google.com", (request) =>
                {
                    Debug.Log(request.result);
                    if(request.result == UnityWebRequest.Result.ConnectionError){
                        Debug.Log("error");
                        //TODO: Pop up message.
                        this.gameObject.SetActive(false);
                    }
                })
            );
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
        private IEnumerator SendRequest(UnityWebRequest request, Action<UnityWebRequest> OnResult){
            yield return request.SendWebRequest();
            OnResult?.Invoke(request);
            request.Dispose();
        }
        public void GetAudioClip(string url, Action<AudioClip> OnResult, AudioType type = AudioType.MPEG){
            StartCoroutine(
                SendRequest(UnityWebRequestMultimedia.GetAudioClip(url, type),
                (req)=>{
                    if(req.result == UnityWebRequest.Result.Success){
                        OnResult?.Invoke(DownloadHandlerAudioClip.GetContent(req));                    
                }
            }));
        }
    }
}