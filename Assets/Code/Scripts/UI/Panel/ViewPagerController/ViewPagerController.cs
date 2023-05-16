using UnityEngine;
using Gameframe.GUI.Extensions;
using System.Collections.Generic;
using System;

namespace Project.UI.Panel{
    public class ViewPagerController : MonoBehaviour{
        public enum SetupEventEnum{
            Start, Manual
        }
        [SerializeField] protected SetupEventEnum setupStartAt = SetupEventEnum.Start;
        [SerializeField] private int cacheCount = 1;
        [SerializeField] PreloadablePanelView[] Samples;
        [SerializeField] protected PreloadablePanelView LastPageView;

        protected int currentIndex = 0;
        public int CurrentIndex => currentIndex;
        protected int cacheIndex;

        protected List<PreloadablePanelView> preloadList;
        public List<PreloadablePanelView> PreloadList => preloadList;
        public event System.Action<PreloadablePanelView> OnPageChanged;
        protected void InvokeOnPageChanged(int index) => OnPageChanged?.Invoke(preloadList[index]);
        protected virtual void Awake(){
            preloadList = new List<PreloadablePanelView>(Math.Max(5, Samples.Length));
            for(int i = 0; i < Samples.Length; i++){
               preloadList.Add(Samples[i]);
            }
        }

        private void Start(){
            if(setupStartAt == SetupEventEnum.Start){
                SetupList();
            }
        }

        protected virtual void AddLastView(){
            if(LastPageView == null) return;
            var newObj = Instantiate(LastPageView, this.transform);
            preloadList.Add(newObj);
        }

        protected virtual void SetupList(){}
        public void ManualSetup(){
            if(setupStartAt == SetupEventEnum.Manual){
                SetupList();
            }
            else{
                Debug.Log("You don't have permission to Setup the list");
            }
        }

        public void ShouldLoadMore(){
            if(Math.Abs(currentIndex - cacheIndex) < cacheCount){
                return;
            }
            //Preload pages ahead
            LoadMore();
        }

        public void LoadMore(){
            int minIndex = Math.Max(currentIndex - cacheCount,0);
            int maxIndex = Math.Min(currentIndex + cacheCount, preloadList.Count - 1);

            for(int i = minIndex; i <= maxIndex; i++){
                StartCoroutine(preloadList[i].PrepareAsync());
            }
            for(int i = 0; i < minIndex; i++){
                StartCoroutine(preloadList[i].UnloadAsync());
            }
            for(int i = maxIndex + 1; i < preloadList.Count; i++){
                StartCoroutine(preloadList[i].UnloadAsync());
            }
            cacheIndex = currentIndex;
        }

        public virtual async void MoveNext(){
            if(!CanMoveNext()) return;

            await preloadList[currentIndex].HideAsync();
            ++currentIndex;
            ShouldLoadMore();
            InvokeOnPageChanged(currentIndex);
            await preloadList[currentIndex].ShowAsync();
        }

        public virtual async void MovePrev(){
            if(!CanMovePrev()) return;

            await preloadList[currentIndex].HideAsync();
            --currentIndex;
            ShouldLoadMore();
            InvokeOnPageChanged(currentIndex);
            await preloadList[currentIndex].ShowAsync();
        }

        public virtual async void MoveTo(int index){
            if(!CanMoveTo(index)) return;

            await preloadList[currentIndex].HideAsync();
            currentIndex = index;
            ShouldLoadMore();
            InvokeOnPageChanged(currentIndex);
            await preloadList[currentIndex].ShowAsync();
        }

        public bool CanMoveNext() => currentIndex + 1 < preloadList.Count;
        public bool CanMovePrev() => currentIndex - 1 >= 0;
        public bool CanMoveTo(int index)=> index >= 0 && index < preloadList.Count;
    }
}