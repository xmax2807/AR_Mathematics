using UnityEngine;
using Project.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    public abstract class ViewPagerControllerT<T> : ViewPagerController where T : PreloadablePanelView
    {
        [SerializeField] private T prefab;
        protected System.Action OnBuildComplete;
        public async Task FetchPanelView(int count, System.Func<T, int, Task> onBuildObj)
        {
            for (int i = 0; i < count; i++)
            {
                await SpawnerManager.Instance.SpawnObjectInParentAsync(prefab, this.transform, async (obj) =>
                { 
                    await obj.HideAsync();
                    preloadList.Add(obj);
                    await onBuildObj?.Invoke(obj, i);
                });
            }
            AddLastView();
        }
    }
}