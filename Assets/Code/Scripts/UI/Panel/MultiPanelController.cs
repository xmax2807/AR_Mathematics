using System.Collections.Generic;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.UI.Panel
{
    public class MultiPanelController : MonoBehaviour
    {
        [SerializeField] PanelViewData ListData;
        [SerializeField] BasePanelController[] Samples;

        private Stack<PanelViewData> cache;
        private Stack<BasePanelController> cacheController;
        public PanelViewData CurrentData => cache.Peek();
        private int currentDataIndex;
        private void Awake()
        {
            cache = new();
            cache.Push(ListData);
            cacheController = new();
        }
        private void Start()
        {
            UpdateUI();
        }
        public void NextUI()
        {
            currentDataIndex = Mathf.Min(currentDataIndex + 1, ListData.Children.Length - 1);
            UpdateUI();
        }
        public void PrevUI()
        {
            currentDataIndex = Mathf.Max(currentDataIndex - 1, 0);
            UpdateUI();
        }
        private void UpdateUI()
        {
            // if (controller != null)
            // {
            //     Pooling.AddToQueue(controller);
            // }
            for (int i = 0; i < CurrentData.ButtonNames.Length; i++)
            {
                int currIndex = i;
                CurrentData.ButtonNames[currIndex].OnClick += () =>
                {
                    cacheController.Peek().Hide();
                    cache.Push(CurrentData.Children[currIndex]);
                    UpdateUI();
                };
            }

            var controller =  Samples.FindMatch((item)=>item.Type == CurrentData.Type);
            
            if(controller == null) return;
            SpawnerManager.Instance.SpawnObject(controller, this.transform.position, this.transform, (obj)=>{
                obj.SetUI(CurrentData);
                cacheController.Push(obj);

                if(obj.BackButton != null) obj.BackButton.interactable = cacheController.Count > 1;
                obj.Show();
            });
        }
    }
}