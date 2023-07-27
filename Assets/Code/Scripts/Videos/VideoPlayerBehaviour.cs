using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Managers;
using UnityEngine;
using UnityEngine.Video;
using Project.AssetIO.Firebase;

public class VideoPlayerBehaviour : MonoBehaviour
{
    public enum VideoState{
        Playing, Pausing, Stopped, Preparing
    }
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private static Camera targetCam;
    public event System.Action<VideoState> OnVideoStateChanged;
    public string VideoUrl { get; set; }

    private RenderTexture texture;

    private void Awake()
    {
        if (targetCam == null) targetCam = Camera.main;
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        if(videoPlayer == null) return;
        //videoPlayer.targetCamera = targetCam;
        videoPlayer.loopPointReached += OnEndPointReached;
        PlayVideo();
    }
    void OnDisable()
    {
        if(videoPlayer == null) return;
        //videoPlayer.targetCamera = null;
        videoPlayer.loopPointReached -= OnEndPointReached;
        PauseVideo();
    }
    
    public void PauseVideo(){
        videoPlayer.Pause();
        OnVideoStateChanged?.Invoke(VideoState.Pausing);
    }
    public void PlayVideo(){
        if(!videoPlayer.isPrepared){
            videoPlayer.Prepare();
        }
        try{
            if(texture != null){
                videoPlayer.targetTexture = texture;
                videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            }
            videoPlayer.Play();
        }
        catch(System.Exception e){
            Debug.Log(e.Message);
        }
        OnVideoStateChanged?.Invoke(VideoState.Playing);
    }
    public void ReplayVideo(){
        videoPlayer.frame = 0;
        PlayVideo();
    }
    private void PrepareVideo(){
        OnVideoStateChanged?.Invoke(VideoState.Preparing);
        videoPlayer.Prepare();
    }
    private void StopVideo(){
        videoPlayer.Stop();
        OnVideoStateChanged?.Invoke(VideoState.Stopped);
    }
    private void OnEndPointReached(VideoPlayer player){
        OnVideoStateChanged?.Invoke(VideoState.Stopped);
    }
    public IEnumerator PrepareVideoUrl(string url)
    {
        if (videoPlayer == null)
        {
            yield return Setup();
        }
        try
        {
            videoPlayer.url = url;
            PrepareVideo();
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        yield break;
    }
    public IEnumerator PrepareVideoClip(VideoClip clip){
        if(videoPlayer == null){
           yield return Setup();
        }

        try
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = clip;
            audioSource ??= AudioManager.Instance.VoiceVolume;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.SetTargetAudioSource(0, audioSource);
            //VideoUrl = clip.originalPath;
            PrepareVideo();
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        yield return null;
    }

    public IEnumerator Setup()
    {
        if (videoPlayer != null)
        {
            Destroy(videoPlayer);
        }
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        if (audioSource == null)
        {
            audioSource = AudioManager.Instance.VoiceVolume;
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = targetCam;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        //videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.frame = 0;
        videoPlayer.prepareCompleted += VideoCompleted;
        yield break;
        // yield return new WaitUntil(()=>VideoUrl != null);
        // yield return PrepareVideoUrl(VideoUrl);
    }
    public IEnumerator Unload()
    {
        if (videoPlayer != null)
        {
            StopVideo();
            videoPlayer.prepareCompleted -= VideoCompleted;
            Destroy(videoPlayer);
        }
        yield return null;
    }

    private void PlayVideo(VideoPlayer video){
        if(!enabled) return;

        PlayVideo();
    }

    private void VideoCompleted(VideoPlayer player){
        player.time = 0f;
    }

    public void AddRenderTexture(RenderTexture texture){
        this.texture = texture;
        if(videoPlayer == null || texture == null){
            return;
        }
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = texture;
        //videoPlayer.targetCamera = targetCam;
    }
}
