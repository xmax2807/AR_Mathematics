using System;
using UnityEngine;
using System.Collections.Generic;

namespace Project.Utils.ExtensionMethods
{
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// Ensure object is not null
        /// </summary>
        /// <param name="obj"></param>
        public static void Gaurd(this GameObject obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"{obj.GetType()} - {obj.name}");
            }
        }

        /// <summary>
        /// Cast object to generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>casted object</returns>
        public static T CastTo<T>(this object obj)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        /// <summary>
        /// Try cast object to generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>false if can't cast, vice versa</returns>
        public static bool TryCastTo<T>(this object obj, out T result){
            result = obj.CastTo<T>();

            return !EqualityComparer<T>.Default.Equals(result, default);
        }

        /// <summary>
        /// Ensure component is not null with reference param
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="component"> ref param </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>this monobehaviour</returns>
        public static MonoBehaviour EnsureComponent<T>(this MonoBehaviour obj, ref T component) where T : Component
        {
            if (component == null)
            {
                component = EnsureComponent<T>(obj);
            }
            return obj;
        }

        /// <summary>
        /// Ensure 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>component or throw if can't be found</returns>
        public static T EnsureComponent<T>(this MonoBehaviour obj) where T : Component
        {
            bool res = obj.TryGetComponent<T>(out var component);

            if (res == false)
            {
                throw new NullReferenceException($"{component.GetType()} - {obj.name}");
            }
            return component;
        }

        public static GameObject AddChildWithComponent(this GameObject obj,string name, Component[] components){
           
            GameObject child = UnityEngine.Object.Instantiate(new GameObject(), obj.transform);
            child.name = name;
            foreach(Component comp in components){
                child.AddComponent(comp.GetType());
            }
            return child;
        }
        public static T AddChildWithComponent<T>(this GameObject obj, string name) where T : Component{
            GameObject child = UnityEngine.Object.Instantiate(new GameObject(), obj.transform);
            child.name = name;
            return child.AddComponent<T>();
        }

        public static void EnsureChildComponent<T>(this GameObject obj, string childName = "Game Object") where T : Component{
            bool hasChild = obj.transform.ContainChildWithComponent<T>();

            if(hasChild) return;

            obj.AddChildWithComponent<T>(childName);
        }
        public static bool ContainChildWithComponent<T>(this Transform transform){
            foreach(Transform child in transform){
                if(child.GetComponent<T>() != null) return true;
            }
            return false;
        }
    }
}
