using System.Collections;
using System;
using UnityEngine;

namespace Project.Utils.ExtensionMethods
{
    public static class ObjectExtensionMethods
    {
        public static void Gaurd(this GameObject obj){
            if(obj == null){
                throw new NullReferenceException($"{obj.GetType()} - {obj.name}");
            }
        }
        public static void EnsureComponent<T>(this MonoBehaviour obj, ref T component) where T : Component{
            if(component == null){
                bool res = obj.TryGetComponent<T>(out component);
                
                if(res == false){
                    throw new NullReferenceException($"{component.GetType()} - {obj.name}");
                }
            }
        }
    }
}
