using System.Collections.Generic;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using UnityEngine;
using Gameframe.GUI.PanelSystem;

namespace Project.UI.Panel
{
    public class MultiPanelController : MonoBehaviour
    {
        [SerializeField] PanelViewData ListData;
        [SerializeField] BasePanelController[] Samples;
        class PanelPack{
            public PanelViewData Data;
            public BasePanelController Controller;
            public PanelPack(PanelViewData data, BasePanelController controller){
                Data = data;
                Controller = controller;
            }
        }
        private Dictionary<string, PanelPack> cached;
        private Stack<PanelPack> stack;
        private PanelPack CurrentData => stack.Count == 0? null : stack.Peek();
        private void Awake()
        {
            stack = new();
            cached = new();
            CreateUI(ListData);
            PushUI(ListData);
        }
        private void CreateUI(PanelViewData data)
        {
            for (int i = 0; i < data.ButtonNames.Length; i++)
            {
                int currIndex = i;
                data.ButtonNames[currIndex].OnClick += () =>
                {
                    PushUI(data.Children[currIndex]);
                };
            }

            var controller = Samples.FindMatch((item) => item.Type == data.Type);

            if (controller == null) return;
            SpawnerManager.Instance.SpawnObject(controller, this.transform.position, this.transform, (obj) =>
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
        private void PopUI()
        {
            if(!stack.TryPop(out var result)) return;
            result.Controller.Hide();

            CurrentData?.Controller.Show();
        }
        private void PushUI(PanelViewData data)
        {
            if(!cached.ContainsKey(data.name)){
                CreateUI(data);
            }

            var pack = cached[data.name];
            pack.Controller.Show();
            CurrentData?.Controller.Hide();
            stack.Push(pack);
        }
    }
}