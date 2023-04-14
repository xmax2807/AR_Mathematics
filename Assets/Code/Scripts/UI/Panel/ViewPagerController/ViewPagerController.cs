using UnityEngine;
using Project.Managers;
using System.Collections.Generic;
using System;

namespace Project.UI.Panel{
    public class ViewPagerController : MonoBehaviour{
        [SerializeField] private int cacheCount = 1;
        [SerializeField] PreloadablePanelView[] Samples;
        [SerializeField] protected PreloadablePanelView LastPageView;
        
        protected int currentIndex = 0;
        protected int cacheIndex;

        protected List<PreloadablePanelView> preloadList;
        public event System.Action<PreloadablePanelView> OnPageChanged;
        protected void InvokeOnPageChanged(int index) => OnPageChanged?.Invoke(preloadList[index]);
        protected virtual void Awake(){
            preloadList = new List<PreloadablePanelView>(Math.Max(5, Samples.Length));
            for(int i = 0; i < Samples.Length; i++){
               preloadList.Add(Samples[i]);
            }
        }

        protected virtual void AddLastView(){
            if(LastPageView == null) return;
            var newObj = Instantiate(LastPageView, this.transform);
            preloadList.Add(newObj);
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

        public bool CanMoveNext() => currentIndex + 1 < preloadList.Count;
        public bool CanMovePrev() => currentIndex - 1 >= 0;
    }
}