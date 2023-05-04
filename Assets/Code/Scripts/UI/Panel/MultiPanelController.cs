using System.Collections.Generic;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using UnityEngine;
using Gameframe.GUI.Camera.UI;

namespace Project.UI.Panel
{
    public class MultiPanelController : MonoBehaviour
    {
        [SerializeField] PanelViewData ListData;
        [SerializeField] BasePanelController[] Samples;
        [SerializeField] UIEventManager eventManager;
        class PanelPack
        {
            public PanelViewData Data;
            public BasePanelController Controller;
            public PanelPack(PanelViewData data, BasePanelController controller)
            {
                Data = data;
                Controller = controller;
            }
        }
        private Dictionary<string, PanelPack> cached;
        private Stack<PanelPack> stack;
        private PanelPack CurrentData => stack.Count == 0 ? null : stack.Peek();
        private void Awake()
        {
            stack = new();
            cached = new();
            CreateUI(ListData);
            PushUI(ListData);
        }
        private void CreateUI(PanelViewData data)
        {
            int min = Mathf.Min(data.Children.Length, data.ButtonNames.Length);
            for (int i = 0; i < min; i++)
            {
                int currIndex = i;
                data.ButtonNames[currIndex].OnClick.AddListener(() => PushUI(data.Children[currIndex]));
            }

            var controller = Samples.FindMatch((item) => item.CheckType(data));

            if (controller == null) return;
            SpawnerManager.Instance.SpawnObjectInParent(controller, this.transform, (obj) =>
            {
                obj.SetUI(data);

                if (obj.BackButton != null)
                {
                    obj.BackButton.interactable = stack.Count > 0;
                    obj.BackButton.onClick.AddListener(PopUI);
                }
                var newPanel = new PanelPack(data, obj);
                cached.Add(data.name, newPanel);
            });
        }
        private async void PopUI()
        {
            eventManager?.Lock();

            {
                if (!stack.TryPop(out var result)) return;

                await result.Controller.Hide();
                if(CurrentData != null){
                    await CurrentData.Controller.Show();
                }
            }

            eventManager?.Unlock();
        }
        private async void PushUI(PanelViewData data)
        {
            if (!cached.ContainsKey(data.name))
            {
                CreateUI(data);
            }
            eventManager?.Lock();

            {
                var pack = cached[data.name];

                if(CurrentData != null){
                    await CurrentData.Controller.Hide();
                }
                await pack.Controller.Show();

                stack.Push(pack);
            }

            eventManager?.Unlock();
        }
    }
}