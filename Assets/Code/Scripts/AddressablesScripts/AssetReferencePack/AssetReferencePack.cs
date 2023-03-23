using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Project.Addressable{
    [Serializable]
    public class AssetReferenceSingle<T> where T : UnityEngine.Object{
        public AssetReferenceT<T> Reference;
        public UnityEvent<T> OnComplete;
    }  

    [Serializable]
    public class AssetReferencePack<T> where T : UnityEngine.Object{
        public AssetReferenceT<T>[] References;
        public UnityEvent<T[]> OnComplete;
    }

    [Serializable]
    public class GameObjectReferencePack : AssetReferencePack<GameObject>{}
    [Serializable]
    public class AudioReferencePack : AssetReferencePack<UnityEngine.AudioClip> {}
}