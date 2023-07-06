using System.Collections;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Project.AppPermission
{
    public enum AppPermission
    {
        Camera = 0,
        Internet = 1,
    }
    public static class RequestPermissionManager
    {
        public static bool AskedPermission = false;
        public static void AskForPermission(AppPermission permission)
        {
            AskedPermission = false;
#if UNITY_IOS
            AskForIOS(permission);
#elif UNITY_ANDROID
            AskForAndroid(permission);
#endif
        }

#if UNITY_IOS
        private static void AskForIOS(AppPermission permission)
        {
            UserAuthorization authorization = permission switch
            {
                AppPermission.Camera => UserAuthorization.WebCam,
                _ => UserAuthorization.WebCam,
            };

            if (Application.HasUserAuthorization(authorization))
            {
                AskedPermission = true;
                return;
            }
            Managers.GameManager.Instance.StartCoroutine(AskPermissionIOS(authorization, PermissionCallback));
        }

        private static IEnumerator AskPermissionIOS(UserAuthorization authorization,System.Action<UserAuthorization> callback){
            yield return Application.RequestUserAuthorization(authorization);
            callback?.Invoke(authorization);
        }

        private static void PermissionCallback(UserAuthorization authorization){
            if (Application.HasUserAuthorization(authorization))
            {
                Debug.Log("granted cam");
            }
            else
            {
                Debug.Log("Failed granted");
            }
            AskedPermission = true;
        }
#endif

#if UNITY_ANDROID
        private static void AskForAndroid(AppPermission permission)
        {

            string authorization = permission switch
            {
                AppPermission.Camera => Permission.Camera,
                _ => Permission.Camera,
            };
            
            if (Permission.HasUserAuthorizedPermission(authorization))
            {
                // The user authorized use of the microphone.
                Debug.Log("Authorized " + authorization);
                AskedPermission = true;
            }
            else
            {
                // We do not have permission to use the microphone.
                // Ask for permission or proceed without the functionality enabled.
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(authorization, callbacks);
            }
        }

        internal static void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
            AskedPermission = true;
        }

        internal static void PermissionCallbacks_PermissionGranted(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
            AskedPermission = true;
        }

        internal static void PermissionCallbacks_PermissionDenied(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
            AskedPermission = true;
        }
#endif
    }
}