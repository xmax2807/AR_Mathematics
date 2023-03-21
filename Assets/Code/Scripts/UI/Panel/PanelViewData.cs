using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.UI.Panel{

    [System.Serializable]
    public class ButtonData{
        public string Name;
        public Button.ButtonClickedEvent OnClick;
        public string Description;
    }
    public enum PanelEnumType{
        Menu, Grid, 
    }
    public abstract class PanelViewData : ScriptableObject{
        public abstract PanelEnumType Type {get;}
        [SerializeField] public PanelViewData[] Children;
        public ButtonData[] ButtonNames;
    }
}