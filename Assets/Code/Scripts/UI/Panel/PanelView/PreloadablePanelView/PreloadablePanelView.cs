using System.Collections;
using Gameframe.GUI.PanelSystem;
using UnityEngine;
namespace Project.UI.Panel{
    public abstract class PreloadablePanelView : PanelView, IPreloadablePanel
    {
        protected bool isPrepared;
        public event System.Action<PreloadablePanelView> OnPrepared;
        public event System.Action<PreloadablePanelView> OnUnloaded;
        public abstract IEnumerator PrepareAsync();
        public abstract IEnumerator UnloadAsync();
    }
}