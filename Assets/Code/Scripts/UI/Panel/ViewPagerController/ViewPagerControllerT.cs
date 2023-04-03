using UnityEngine;
using Project.Managers;
namespace Project.UI.Panel
{
    public abstract class ViewPagerControllerT<T> : ViewPagerController where T : PreloadablePanelView
    {
        [SerializeField] private T prefab;

        public void Setup(int count, System.Action<T, int> onBuildObj)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnerManager.Instance.SpawnObjectInParent(prefab, this.transform, (obj) =>
                {
                    onBuildObj?.Invoke(obj, i);
                    obj.HideAsync();
                    preloadList.Add(obj);
                });
            }
        }
    }
}