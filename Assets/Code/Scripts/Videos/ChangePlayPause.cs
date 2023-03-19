using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class ChangePlayPause : MonoBehaviour
{
    private VideoPlayer player;
    public Button button;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeStartStop()
    {
        if (player.isPlaying == false)
        {
            player.Play();
            image.enabled = false;
        }
        else
        {
            player.Pause();
            image.enabled = true;
        }
    }
}
