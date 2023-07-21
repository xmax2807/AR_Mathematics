using System.Collections.Generic;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using UnityEngine;
using Gameframe.GUI.Camera.UI;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    public class MultiPanelController : MonoBehaviour
    {
        [SerializeField] PanelViewData ListData;
        [SerializeField] BasePanelController[] Samples;
        [SerializeField] UIEventManager eventManager;
        [SerializeField] private UnityEngine.UI.Button backButton;
        [SerializeField] private Transform mainViewTransform;
        private HistoryPanelDataSO historySO;
        private HistoryPanelDataSO HistoryPanelDataSO
        {
            get
            {
                if (historySO == null)
                {
                    historySO = ResourceManager.Instance.HistorySO;
                }
                return historySO;
            }
        }
        private Dictionary<PanelViewData, BasePanelController> cached;
        private Dictionary<PanelViewData, UnityEngine.Events.UnityAction[]> cachedDelegates;
        private PanelViewData CurrentData => HistoryPanelDataSO.CurrentData;

        private void Start()
        {
            cached = new();
            cachedDelegates = new();
            if (HistoryPanelDataSO.Count == 0)
            {
                Debug.Log("History is 0");
                PushUI(ListData);
            }
            else
            {
                // PanelViewData[] history = HistoryPanelDataSO.History;
                // for (int i = 0; i < history.Length; i++)
                // {
                //     PushUI(history[i]);
                // }

                historySO.TryPop(out PanelViewData data);
                PushUI(data);
            }
        }

        private void OnDestroy(){
            foreach(var pair in cached){
                Destroy(pair.Value);
            }
            cached.Clear();

            foreach(var pair in cachedDelegates){
                ButtonData[] buttonDatas = pair.Key.ButtonNames;
                UnityEngine.Events.UnityAction[] actions = pair.Value;
                for(int i = 0; i < actions.Length; ++i){
                    // unsubscribe onclick
                    buttonDatas[i].OnClick.RemoveListener(pair.Value[i]);   
                }
            }
        }
        private void OnEnable()
        {
            backButton?.onClick.AddListener(PopUI);
        }
        private void OnDisable()
        {
            backButton?.onClick.RemoveListener(PopUI);
        }
        private void CreateUI(PanelViewData data)
        {
            if(cachedDelegates.ContainsKey(data)){
                SpawnViewController(data);
                return;
            }

            int min = Mathf.Min(data.Children.Length, data.ButtonNames.Length);
            cachedDelegates[data] = new UnityEngine.Events.UnityAction[min];
            for (int i = 0; i < min; i++)
            {
                int currIndex = i;
                cachedDelegates[data][i] = () => PushUI(data.Children[currIndex]);
                data.ButtonNames[currIndex].OnClick.AddListener(cachedDelegates[data][i]);
            }

            SpawnViewController(data);
        }

        private void SpawnViewController(PanelViewData data)
        {
            var controller = Samples.FindMatch((item) => item.CheckType(data));

            if (controller == null) return;

            var obj = Instantiate(controller, mainViewTransform, false);
            obj.SetUI(data);
            cached[data] = obj;
        }

        private async void PopUI()
        {
            eventManager?.Lock();

            if (!HistoryPanelDataSO.TryPop(out var data))
            {
                eventManager?.Unlock();
                ShouldEnableBackButton();
                return;
            }

            ShouldEnableBackButton();
            if (cached.TryGetValue(data, out var controller))
            {
                
                await controller.Hide();
            }

            PanelViewData currentData = CurrentData;
            if (currentData != null)
            {
                // If stack has the data but the cached has not spawned the controller => spawn only (not create UI)
                if(!cached.ContainsKey(currentData)){
                    CreateUI(currentData);
                }

                await cached[currentData].Show();
            }

            eventManager?.Unlock();
        }
        private async void PushUI(PanelViewData data)
        {
            if (data == null) return;

            eventManager?.Lock();

            //Hide first
            PanelViewData currentData = CurrentData;
            if (currentData != null)
            {
                bool result = cached.TryGetValue(currentData, out BasePanelController controller);
                Debug.Log(result);
                if(result == true){
                    await controller.Hide();
                }
            }

            // Start Pushing new one

            // stack doesn't have any data and also cache hasn't spawned the controller.
            if (!cached.ContainsKey(data))
            {
                CreateUI(data);
            }

            HistoryPanelDataSO.Push(data);
            ShouldEnableBackButton();

            await cached[data].Show();

            eventManager?.Unlock();
        }

        private void ShouldEnableBackButton()
        {
            if (backButton != null)
            {
                backButton.interactable = HistoryPanelDataSO.Count > 1;
            }
        }
    }
}