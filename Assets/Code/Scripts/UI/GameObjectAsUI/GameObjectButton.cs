namespace Project.UI.GameObjectUI{
    public class GameObjectButton : UnityEngine.MonoBehaviour, ITouchableObject
    {
        private string id;
        public string UniqueID => id;

        public void OnEnable(){

            //Create new Guid
            id = System.Guid.NewGuid().ToString();
            
        }

        public bool Equals(ITouchableObject other)
        {
            return other.UniqueID == this.UniqueID;
        }

        public void OnTouch(UnityEngine.TouchPhase phase)
        {
            
        }
    }
}