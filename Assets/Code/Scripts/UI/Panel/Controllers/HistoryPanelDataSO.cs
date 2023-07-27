using System.Collections.Generic;
using UnityEngine;
namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "HistoryPanelData", menuName = "STO/PanelViewData/HistoryPanelData")]
    public class HistoryPanelDataSO : ScriptableObject{
        private List<PanelViewData> _history = new();
        public PanelViewData[] History => _history.ToArray();
        private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
        internal int Count => _history.Count;
        internal PanelViewData CurrentData => _history.Count == 0 ? null : _history[Count - 1];
        internal bool TryPop(out PanelViewData result){
            result = CurrentData; 
            if(result == null){
                return false;
            }
            _history.RemoveAt(Count - 1);
            return true;
        }
        internal void Push(PanelViewData data){
            _history.Add(data);
        }

        internal void Clear(){
            _history.Clear();
        }
    }
}