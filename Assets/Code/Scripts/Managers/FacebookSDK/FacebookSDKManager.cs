using System;
using System.Collections;
using Facebook.Unity;
using UnityEngine;

namespace Project.Managers.FacebookSDK
{
    public class FacebookSDKManager
    {
        private static FacebookSDKManager m_instance;
        public static FacebookSDKManager Instance{
            get {
                m_instance ??= new();
                return m_instance;
            }
        }

        public FacebookFeedController FeedController {get;private set;}

        private FacebookSDKManager()
        {
            FB.Init(OnInitComplete, OnHideUnity);
            FeedController = new();
        }

        private void OnHideUnity(bool isUnityShown)
        {
            // Handle the game's visibility change
            Debug.Log("Game visibility changed: " + isUnityShown);
        }

        private void OnInitComplete()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                //isInitialized = true;
                Debug.Log("Facebook SDK initialized successfully.");
            }
            else
            {
                Debug.LogError("Failed to initialize Facebook SDK.");
            }
        }

        public IEnumerator LogoutCurrentAccount(){
            yield return new WaitUntil(()=>FB.IsInitialized);
            if (FB.IsLoggedIn){
                FB.LogOut();
            }
        }
    }
}