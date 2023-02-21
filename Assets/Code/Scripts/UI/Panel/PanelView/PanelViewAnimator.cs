using UnityEngine;
using Project.Utils.ExtensionMethods;

namespace Project.UI.Panel{
    [RequireComponent(typeof(Animator))]
    public class PanelViewAnimator : BasePanelView{
        [SerializeField, HideInInspector] protected Animator _animator;
        
        public void Awake(){
            this.EnsureComponent(ref _animator);
        }
    }
}