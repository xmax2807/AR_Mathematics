using UnityEngine;
namespace Project.MiniGames
{
    public abstract class SingletonEventManager<TManager> : MonoBehaviour where TManager : MonoBehaviour
    {
        private static TManager _instance;
        public static TManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("Manager");
                    _instance = managerObject.AddComponent<TManager>();
                }
                return _instance;
            }
        }
        protected virtual void Awake(){
            _instance = this.GetComponent<TManager>();
        }
    }
}