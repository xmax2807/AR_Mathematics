using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Project.Addressable{

    [Serializable]
    public class AssetReferencePack{
        public AssetReference[] references;
        public UnityEvent<GameObject[]> OnComplete;
    }

    [Serializable]
    public class AssetReferencePack<T> where T : UnityEngine.Object{
        public AssetReferenceT<T>[] references;
        public UnityEvent<T[]> OnComplete;
    }

    [Serializable]
    public class AssetReferenceAudio : AssetReferencePack<UnityEngine.AudioClip> {}
}