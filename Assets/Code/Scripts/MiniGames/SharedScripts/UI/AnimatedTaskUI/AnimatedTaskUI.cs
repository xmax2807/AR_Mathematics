using System;
using System.Threading.Tasks;

using UnityEngine;
using Gameframe.GUI.PanelSystem;
namespace Project.MiniGames.UI{
    [RequireComponent(typeof(AnimatedPanelView))]
    public class AnimatedTaskUI : BaseTaskUI, IAnimatableTaskUI, ITaskUIWrappee
    {
        protected AnimatedPanelView view;
        public AnimatedPanelView View => view;
        public event Action OnTaskShowed;
        public event Action OnTaskHidden;
        [SerializeField] protected TaskUIWrappee wrappee;

        protected virtual void Awake(){
            view = GetComponent<AnimatedPanelView>();
        }

        public Task HideAsync()
        {
            OnTaskHidden?.Invoke();
            return view.HideAsync();
        }

        public Task ShowAsync()
        {
            OnTaskShowed?.Invoke();
            return view.ShowAsync();
        }
        public void Hide(){
            HideAsync();
        }
        public void Show(){
            ShowAsync();
        }

        protected override void UpdateUI(BaseTask task)
        {
            wrappee.ManuallyUpdateUI(task);
        }

        public void ManuallyUpdateUI(BaseTask task)
        {
            wrappee.ManuallyUpdateUI(task);
        }
    }
}