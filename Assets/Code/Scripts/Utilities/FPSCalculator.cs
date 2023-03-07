using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCalculator : MonoBehaviour
{
    int frameCount = 0;
    float dt = 0f;
    float fps = 0f;
    float invUpdateRate = 1f/4f;  // 4 updates per sec.
     private GUIStyle guiStyle = new GUIStyle();

    void Start(){
        guiStyle.fontSize = 36;
        guiStyle.normal.textColor = Color.white;
    }
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > invUpdateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= invUpdateRate;
        }
    }

    void OnGUI(){
        GUI.Label(new Rect(50, 100, 300, 80), fps.ToString(), guiStyle);
    }
}