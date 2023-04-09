using UnityEngine;
using Project.Managers;
using System.Collections.Generic;
using System;

namespace Project.UI.Panel{
    public class ViewPagerController : MonoBehaviour{
        [SerializeField] private int cacheCount = 1;
        [SerializeField] PreloadablePanelView[] Samples;
        
        protected int currentIndex = 0;
        protected int cacheIndex;
        protected List<PreloadablePanelView> preloadList;  
        public event System.Action<int> OnPageChanged;
        protected virtual void Awake(){
            preloadList = new List<PreloadablePanelView>(Math.Max(5, Samples.Length));
            for(int i = 0; i < Samples.Length; i++){
               preloadList.Add(Samples[i]); 
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
            int maxIndex = Math.Min(cacheCount, preloadList.Count - 1 - currentIndex);

            for(int i = minIndex; i <= maxIndex; i++){
                StartCoroutine(preloadList[i + currentIndex].PrepareAsync());
            }
            for(int i = 0; i < minIndex + currentIndex; i++){
                StartCoroutine(preloadList[i].UnloadAsync());
            }
            for(int i = maxIndex + 1 + currentIndex; i < preloadList.Count; i++){
                StartCoroutine(preloadList[i].UnloadAsync());
            }
            cacheIndex = currentIndex;
        }

        public virtual async void MoveNext(){
            if(!CanMoveNext()) return;

            await preloadList[currentIndex].HideAsync();
            ++currentIndex;
            ShouldLoadMore();
            OnPageChanged?.Invoke(currentIndex + 1);
            await preloadList[currentIndex].ShowAsync();
        }

        public virtual async void MovePrev(){
            if(!CanMovePrev()) return;

            await preloadList[currentIndex].HideAsync();
            --currentIndex;
            ShouldLoadMore();
            OnPageChanged?.Invoke(currentIndex - 1);
            await preloadList[currentIndex].ShowAsync();
        }

        public bool CanMoveNext() => currentIndex + 1 < preloadList.Count;
        public bool CanMovePrev() => currentIndex - 1 >= 0;
    }
}