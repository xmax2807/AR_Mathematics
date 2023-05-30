using UnityEngine;
namespace Project.MiniGames{
    public abstract class BaseTaskUI : UnityEngine.MonoBehaviour{
        public bool IsDebugging = false;
        [SerializeField] protected TaskGiver giver;

        protected virtual void OnEnable(){
            if(giver != null){
                giver.OnTaskChanged += UpdateUI;
            }
        }
        protected virtual void OnDisable(){
            if(giver != null){
                giver.OnTaskChanged -= UpdateUI;
            }
        }
        protected virtual void Start(){
            // if(giver == null){
            //     this.enabled = false;
            // }
        }

        protected abstract void UpdateUI(BaseTask task);
    } 
}