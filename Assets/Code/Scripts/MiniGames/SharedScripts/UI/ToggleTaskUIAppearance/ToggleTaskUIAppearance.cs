using System;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using UnityEngine;
namespace Project.MiniGames.UI{
    public class ToggleTaskUIAppearance : BaseTaskUI{
        [SerializeField] private AnimatedTaskUI MainTaskUI;
        [SerializeField] private AnimatedTaskUI SecondTaskUI;
        private UIEventManager eventManager;

        private AnimatedTaskUI currentUI;
        private bool shouldShowMainFirst = false;

        protected override async void Start(){
            eventManager = UIEventManager.Current;
            MainTaskUI?.View.HideImmediate();
            SecondTaskUI?.View.HideImmediate();
            currentUI = shouldShowMainFirst ? MainTaskUI : SecondTaskUI;
            if(currentUI != null){
                await currentUI.ShowAsync();
            }
        }

        public async void ToggleUI()
        {
            shouldShowMainFirst = !shouldShowMainFirst;
            eventManager.Lock();

            Task task = Task.CompletedTask;
            if(currentUI != null){
                task = currentUI.HideAsync();
            }
            currentUI = shouldShowMainFirst ? MainTaskUI : SecondTaskUI;
            if(currentUI != null){
                task = currentUI.ShowAsync();
            }

            await task;
            eventManager.Unlock();
        }

        protected override void UpdateUI(BaseTask task)
        {
            MainTaskUI?.ManuallyUpdateUI(task);
            SecondTaskUI?.ManuallyUpdateUI(task);
        }
    }
}