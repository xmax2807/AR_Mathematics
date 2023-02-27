using System;
using Project.Utils.ExtensionMethods;
using System.Linq;
using UnityEngine;

namespace Project.Utils
{
    public class Observer : MonoBehaviour
    {
        public event Action<bool> OnToggleVisible;
        public void OnBecameInvisible() => OnToggleVisible?.Invoke(false);
        public void OnBecameVisible() => OnToggleVisible?.Invoke(true);
    }
}