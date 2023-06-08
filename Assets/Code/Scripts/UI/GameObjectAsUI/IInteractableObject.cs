namespace Project.UI.GameObjectUI{

    public interface ITouchableObject : System.IEquatable<ITouchableObject>{
        void OnTouch(UnityEngine.TouchPhase phase);
        string UniqueID {get;}
    }
}