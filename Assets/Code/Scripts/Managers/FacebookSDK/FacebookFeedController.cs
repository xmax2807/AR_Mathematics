using System;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

namespace Project.Managers.FacebookSDK
{
    public class FacebookFeedController
    {
        public async void PostFeed(byte[] imageContent)
        {
            if(!FB.IsInitialized){
                TimeCoroutineManager.Instance.WaitUntil(()=>FB.IsInitialized, ()=>PostFeed(imageContent));
                return;
            }

            if (!FB.IsLoggedIn)
            {
                FB.LogInWithPublishPermissions(callback:
                 (loginResult) =>
                 {
                     if (loginResult.Error != null)
                     {
                         Debug.Log(loginResult.Error);
                         return;
                     }
                     PostFeed(imageContent);
                 }
                );

                return;
            }
            if (FB.IsLoggedIn)
            {
                Uri uri = await DatabaseManager.Instance.UserController.UploadImageBytes(UserManager.Instance.CurrentUser.UserID, imageContent);
                if(uri.AbsolutePath == ""){
                    return;
                }
                
                FB.FeedShare(picture:uri, linkCaption: "My awesome photo!", callback: OnFeedPostComplete);
                // var wwwForm = new WWWForm();
                // wwwForm.AddBinaryData("image", imageContent, "picture.png");
                // FB.API("me/photos", HttpMethod.POST, OnPhotoUploadComplete, wwwForm);
                //FB.ShareLink(new Uri("https://www.google.com.vn/"), callback: OnShareComplete);
            }
            else
            {
                Debug.LogWarning("User is not logged in to Facebook.");
            }
        }

        private void OnShareComplete(IShareResult result)
        {
            if (result.Error != null){
                Debug.Log(result.Error);
            }
            else{
                Debug.Log("Share complete.");
            }
        }

        // private void OnPhotoUploadComplete(IGraphResult result)
        // {
        //     if (result.Error != null)
        //     {
        //         Debug.LogError($"Failed to upload photo to Facebook: {result.Error}");
        //     }
        //     else
        //     {
        //         string photoId = result.ResultDictionary["id"].ToString();

        //         // Use the obtained photo ID to create a feed post with a caption
        //         var feedParams = new Dictionary<string, string>()
        //     {
        //         { "message", "My awesome photo!" },
        //         { "link", $"https://www.facebook.com/photo.php?fbid={photoId}" }
        //     };

        //         FB.API("me/feed", HttpMethod.POST, OnFeedPostComplete, feedParams);
        //     }

        // }

        private void OnFeedPostComplete(IShareResult result)
        {
            if (result.Error == null)
            {
                Debug.Log("Feed posted successfully.");
            }
            else
            {
                Debug.LogError("Failed to post feed: " + result.Error);
            }
        }
    }
}