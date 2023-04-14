using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.UI.Panel{
    public class PreloadableVideoPanelView : PreloadablePanelView
    {
        [SerializeField] private VideoPlayerBehaviour videoPlayerBehaviour;
        public VideoPlayerBehaviour VideoPlayerBehaviour {get => videoPlayerBehaviour;}
        public override IEnumerator PrepareAsync()
        {
            if(isPrepared) yield break;
            yield return videoPlayerBehaviour.Setup();
            isPrepared = true;
        }

        public override IEnumerator UnloadAsync()
        {
            if(!isPrepared) yield break;
            yield return videoPlayerBehaviour.Unload();
            isPrepared = false;
        }
        public override Task HideAsync(CancellationToken cancellationToken)
        {
            videoPlayerBehaviour.enabled = false;
            return Task.CompletedTask;
        }

        public override Task ShowAsync(CancellationToken cancellationToken)
        {
            videoPlayerBehaviour.enabled = true;
            return Task.CompletedTask;
        }
    }
}