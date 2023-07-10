using UnityEngine;
using Project.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    public abstract class ViewPagerControllerT<T> : ViewPagerController where T : PreloadablePanelView
    {
        [SerializeField] private T prefab;
        public event System.Action OnBuildComplete;
        public async Task FetchPanelView(int count, System.Func<T, int, Task> onBuildObj)
        {
            for (int i = 0; i < count; ++i)
            {
                T obj = SpawnerManager.Instance.SpawnObjectInParent<T>(prefab, this.transform);

                preloadList.Add(obj);
                await onBuildObj?.Invoke(obj, i);
                //obj.HideImmediate();
            }
            AddLastView();
            OnBuildComplete?.Invoke();
        }
        protected virtual Task OnBuildUIView(T item, int index) => Task.CompletedTask;
    }
}