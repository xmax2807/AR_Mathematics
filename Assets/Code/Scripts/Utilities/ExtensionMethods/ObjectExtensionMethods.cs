using System.Collections;
using System;
using UnityEngine;

namespace Project.Utils.ExtensionMethods
{
    public static class ObjectExtensionMethods
    {
        public static void Gaurd(this GameObject obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"{obj.GetType()} - {obj.name}");
            }
        }
        public static MonoBehaviour EnsureComponent<T>(this MonoBehaviour obj, ref T component) where T : Component
        {
            if (component == null)
            {
                component = EnsureComponent<T>(obj);
            }
            return obj;
        }
        public static T EnsureComponent<T>(this MonoBehaviour obj) where T : Component
        {
            bool res = obj.TryGetComponent<T>(out var component);

            if (res == false)
            {
                throw new NullReferenceException($"{component.GetType()} - {obj.name}");
            }
            return component;
        }
    }
}
