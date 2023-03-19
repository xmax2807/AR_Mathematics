using UnityEngine;
using UnityEngine.Events;

namespace Project.UI.Panel{

    [System.Serializable]
    public class ButtonData{
        public string Name;
        public UnityAction OnClick;
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