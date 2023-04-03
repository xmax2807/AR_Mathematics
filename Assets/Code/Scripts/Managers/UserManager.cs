using UnityEngine;

namespace Project.Managers{
    public class UserManager : MonoBehaviour{
        public static UserManager Instance;
        public LessonModel CurrentLesson {get;set;}

        public void Awake(){
            if(Instance == null){
                Instance = this;
            }
            else if(Instance != null && Instance != this){
                Destroy(this.gameObject);
            }
        }

        
    }
}