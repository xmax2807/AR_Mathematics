using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleTextToSpeech : MonoBehaviour
{
    private const string googleURL = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=vi&q=";
    public string words = "Hello";
    [SerializeField] private AudioSource audio;
    IEnumerator GetRequest()
    {
        string url = googleURL + Uri.EscapeDataString(words);

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return www.SendWebRequest();
        
        var clip = DownloadHandlerAudioClip.GetContent(www);
        audio.clip = clip;
        audio.Play();

        www.Dispose();
    }

    void OnGUI()
    {
        words = GUI.TextField(new Rect(Screen.width / 2 - 200 / 2,Screen.height / 2 - 100 / 2, 400, 100), words);
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2, 40, 150, 50), "Speak"))
        {
            StartCoroutine(GetRequest());
        }
    }
}
