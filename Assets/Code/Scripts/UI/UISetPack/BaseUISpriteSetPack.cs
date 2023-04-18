
using System.Threading.Tasks;
using UnityEngine;
namespace Project.UI.UISetPack{
    public abstract class BaseUISpriteSetPack : ScriptableObject{
        public bool IsInitialized {get; protected set;}
        public abstract Task Init();
        protected abstract void OnDestroy();
    }
}