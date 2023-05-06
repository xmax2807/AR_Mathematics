using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.UI.Panel{

    
    public enum PanelEnumType{
        Menu, Grid, Request
    }
    public abstract class PanelViewData : ScriptableObject{
        public abstract PanelEnumType Type {get;}
        [SerializeField] public PanelViewData[] Children;
        public ButtonData[] ButtonNames;
    }
}