namespace Project.UI.GameObjectUI{

    public interface ITouchableObject : System.IEquatable<ITouchableObject>{
        void OnTouch(UnityEngine.Touch touch);
        string UniqueID {get;}
        bool Interactable {get;set;}
    }
}