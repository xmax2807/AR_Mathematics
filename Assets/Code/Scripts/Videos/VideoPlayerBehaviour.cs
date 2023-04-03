using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Managers;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerBehaviour : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private static Camera targetCam;

    public string VideoUrl { get; set; }

    public int unit = 1;
    public int chapter = 1;
    public int videoIndex = 0;

    private void Awake()
    {
        if (targetCam == null) targetCam = Camera.main;
    }

    void OnEnable()
    {
        if(videoPlayer == null) return;
        videoPlayer.targetCamera = targetCam;
        videoPlayer.Play();
    }
    void OnDisable()
    {
        if(videoPlayer == null) return;
        videoPlayer.targetCamera = null;
        videoPlayer.Pause();
    }

    public IEnumerator PrepareVideoUrl(string url)
    {
        if (videoPlayer == null)
        {
            Setup();
        }
        try
        {
            videoPlayer.url = url;
            Debug.Log("got " + url);
            videoPlayer.Prepare();
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        yield return null;
    }

    public IEnumerator Setup()
    {
        Debug.Log("setup");
        if (videoPlayer != null)
        {
            Destroy(videoPlayer);
        }
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        if (audioSource == null)
        {
            audioSource = AudioManager.Instance.BackgroundFX;
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = targetCam;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.frame = 0;
        videoPlayer.prepareCompleted += PlayVideo;
        
        yield return new WaitUntil(()=>VideoUrl != null);
        yield return PrepareVideoUrl(VideoUrl);
    }
    public IEnumerator Unload()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.prepareCompleted -= PlayVideo;
            Destroy(videoPlayer);
        }
        yield return null;
    }

    private void PlayVideo(VideoPlayer video){
        if(!enabled) return;

        video.Play();
    }
}
