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
        public static T CastTo<T>(this object obj, Func<object, InvalidCastException, T> onErrorFallback = null)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (InvalidCastException e)
            {
                if(onErrorFallback == null) return default;

                return onErrorFallback.Invoke(obj, e);
            }
        }

        /// <summary>
        /// Try cast object to generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>false if can't cast, vice versa</returns>
        public static void TryCastTo<T>(this object obj, Func<object, InvalidCastException, T> onErrorFallback, out T result){
            result = obj.CastTo<T>(onErrorFallback);
        }
        public static void TryCastTo<T> (this object obj, out T result){
            result = obj.CastTo<T>();
        }

        /// <summary>
        /// Ensure component is not null with reference param
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="component"> ref param </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>this monobehaviour</returns>
        public static MonoBehaviour EnsureComponent<T>(this MonoBehaviour obj, ref T component, bool autoCreate = false) where T : Component
        {
            if (component == null)
            {
                component = EnsureComponent<T>(obj, autoCreate);
            }
            return obj;
        }

        /// <summary>
        /// Ensure 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>component or throw if can't be found</returns>
        public static T EnsureComponent<T>(this MonoBehaviour obj, bool autoCreate = false) where T : Component
        {
            bool res = obj.TryGetComponent<T>(out var component);

            if (res == false)
            {
                if(autoCreate){
                    return obj.gameObject.AddComponent<T>();
                }
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
            GameObject child = new(name); 
            child.transform.parent = obj.transform;
            return child.AddComponent<T>();
        }

        public static T AddChildWithScript<T>(this GameObject obj, string name) where T : MonoBehaviour{
            GameObject child = new(name); 
            child.transform.parent = obj.transform;
            return child.AddComponent<T>();
        }

        public static GameObject EnsureChildWithName(this GameObject obj, string childName){
            Transform child = obj.transform.GetChildWithName(childName);
            if(child == null){
                child = new GameObject(childName).transform;
                child.SetParent(obj.transform, false);
            }
            return child.gameObject;
        }

        public static T EnsureChildComponent<T>(this GameObject obj, string childName = "Game Object") where T : Component{
            bool hasChild = obj.transform.ContainChildWithComponent<T>();

            if(hasChild) return obj.GetComponentInChildren<T>();

            return obj.AddChildWithComponent<T>(childName);
        }
        public static bool ContainChildWithComponent<T>(this Transform transform){
            foreach(Transform child in transform){
                if(child.GetComponent<T>() != null) return true;
            }
            return false;
        }

        public static Transform GetChildWithName(this Transform transform, string name){
            foreach(Transform child in transform){
                if(child.name == name) return child;
            }
            return null;
        }

        public static Vector3 TryGetObjectSize(this GameObject obj){
            Vector3 result = GetSizeFromRenderer(obj);
            if(result != Vector3.zero){
                return result;
            }

            result = GetSizeFromCollider(obj);
            if(result != Vector3.zero){
                return result;
            }

            return Vector3.zero;
        }

        public static Vector3 GetSizeFromRenderer(this GameObject obj){
            //Get renderer of this obj
            if(obj.TryGetComponent<Renderer>(out Renderer objRenderer)){
                return objRenderer.bounds.size;
            }

            // if not get first renderer in children
            objRenderer = obj.GetComponentInChildren<Renderer>(true);
            if(objRenderer == null){
                Debug.Log($"{obj.name} doesn't have any renderer");
                return Vector3.zero;
            }
            return objRenderer.bounds.size;
        }

        public static Bounds GetBoundsFromRenderer(this GameObject obj){
            //Get renderer of this obj
            if(obj.TryGetComponent<Renderer>(out Renderer objRenderer)){
                return objRenderer.bounds;
            }

            // if not get first renderer in children
            objRenderer = obj.GetComponentInChildren<Renderer>(true);
            if(objRenderer == null){
                Debug.Log($"{obj.name} doesn't have any renderer");
                return default;
            }
            return objRenderer.bounds;
        }

        public static Vector3 GetSizeFromCollider(this GameObject obj){
            if(obj.TryGetComponent<Collider>(out Collider objCollider)){
                return objCollider.bounds.size;
            }

            objCollider = obj.GetComponentInChildren<Collider>(true);
            if(objCollider == null){
                Debug.Log($"{obj.name} doesn't have any renderer");
                return Vector3.zero;
            }
            return objCollider.bounds.size;
        }
    }
}
