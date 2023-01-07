using System.Collections;
using System;
using UnityEngine;

namespace Project.ExtensionMethods
{
    public static class ObjectExtensionMethods
    {
        public static void Gaurd(this GameObject obj){
            if(obj == null){
                throw new NullReferenceException($"{obj.GetType()} - {obj.name}");
            }
        }
    }
}
