using System;
using UnityEngine;

namespace Project.Utils.ObjectPooling{
    public abstract class BasePoolingObject : MonoBehaviour, IPooling
    {
        public virtual string Name => "Base Pooling Object";
        private bool _isUsing;

        public event Action<IPooling> AddBackToQueue;

        public bool IsUsing { get => _isUsing; set => _isUsing = value; }

        public virtual void OnRelease()
        {
            _isUsing = true;
            this.gameObject.SetActive(true);
        }

        public virtual void OnReturn()
        {
            _isUsing = false;
            this.gameObject.SetActive(false);
        }
    }
}