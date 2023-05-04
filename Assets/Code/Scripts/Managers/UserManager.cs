using UnityEngine;

namespace Project.Managers{
    public class UserManager : MonoBehaviour{
        public struct CurrentUnit{
            public int unit;
            public int chapter;
            public int semester;
            public int NextUnit() => unit+1;
            public bool IsEmpty() {
                return chapter <= 0 && unit <= 0;
            }
        }
        public static UserManager Instance;
        public UserModel CurrentUser {get;set;}
        public LessonModel CurrentLesson {get;set;}
        public QuizModel[] CurrentQuizzes;
        public CurrentUnit CurrentUnitProgress;
        public GameModel[] CurrentGameScenes;
        public GameModel CurrentGame;
        public CourseModel CourseModel;
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