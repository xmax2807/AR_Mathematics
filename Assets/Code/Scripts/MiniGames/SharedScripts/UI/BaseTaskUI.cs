using UnityEngine;
namespace Project.MiniGames{
    public abstract class BaseTaskUI : UnityEngine.MonoBehaviour{
        [SerializeField] protected TaskGiver giver;

        protected virtual void OnEnable(){
            giver.OnTaskChanged += UpdateUI;
        }
        protected virtual void OnDisable(){
            giver.OnTaskChanged -= UpdateUI;
        }
        protected void Start(){
            if(giver == null){
                this.enabled = false;
            }
        }

        protected abstract void UpdateUI(BaseTask task);
    } 
}