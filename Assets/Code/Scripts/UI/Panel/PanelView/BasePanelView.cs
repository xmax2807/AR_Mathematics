using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Project.UI.Panel
{
    public abstract class BasePanelView : MonoBehaviour
    {
        protected virtual Task ShowAsync(CancellationToken token){
            gameObject.SetActive(true);
            return Task.CompletedTask;
        }
        protected virtual Task HideAsync(CancellationToken token){
            gameObject.SetActive(false);
            return Task.CompletedTask;
        }

        public Task ShowAsync() => ShowAsync(CancellationToken.None);
        public Task HideAsync() => HideAsync(CancellationToken.None);
    }
}
