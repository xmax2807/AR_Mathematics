using UnityEngine;
namespace Project.UI.Event{
    [CreateAssetMenu(fileName = "UIPagerEventSO", menuName = "STO/UIEvent/UIPagerEventSO")]
    public class UIPagerEventSO : UIEventScriptableObject{
        public void MoveTo(int index){
            if(listeners == null) return;

            int count = listeners.Count;
            for(int i =0 ; i < count; ++i){
                if(listeners[i] is IPagerUIEventListener listener){
                    listener.MoveTo(index);
                }
            }
        }
    }
}